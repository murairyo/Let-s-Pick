using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class ColliderHandlerStage3 : MonoBehaviour
{
    [SerializeField] private GameObject CatInCup; // カップに表示される猫
    [SerializeField] private Material cupInMaterial; // カップイン時の共通マテリアル
    [SerializeField] private List<GameObject> fxPrefabs; // 成功時のFXプレファブリスト
    [SerializeField] private List<AudioSource> seAudioSources; // 成功時のSE
    [SerializeField] private GameObject incorrectFxPrefab; // 不正解時のエフェクト
    [SerializeField] private AudioSource incorrectSe; // 不正解時の音声

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;
    private bool isLocked = false; // 一度成功したら処理をロックする

    public event System.Action OnCupFilled; // カップイン成功時のイベント

    private CombinationManager combinationManager; // CombinationManagerの参照

    void Start()
    {
        // CatInCupのコンポーネントを取得
        skinnedMeshRenderer = CatInCup.GetComponentInChildren<SkinnedMeshRenderer>();
        animator = CatInCup.GetComponent<Animator>();

        // CatInCupを非表示にする
        if (skinnedMeshRenderer != null) skinnedMeshRenderer.enabled = false;
        if (animator != null) animator.enabled = false;

        // CombinationManagerを取得
        combinationManager = FindObjectOfType<CombinationManager>();
        if (combinationManager == null)
        {
            Debug.LogError("CombinationManagerが見つかりません！シーンに配置してください。");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isLocked) return; // 既にロックされている場合は処理を行わない

        if (other.CompareTag("Character"))
        {
            // 組み合わせを確認
            bool isCorrect = CheckCombination(other);

            if (isCorrect)
            {
                isLocked = true; // 成功時にロック
                HandleCorrectCombination(other);
                OnCupFilled?.Invoke(); // 成功イベントを発火
            }
            else
            {
                HandleIncorrectCombination();
            }
        }
    }

    /// <summary>
    /// 組み合わせを確認する
    /// </summary>
    private bool CheckCombination(Collider other)
    {
        // CatTypeを取得
        Cat cat = other.GetComponent<Cat>();
        if (cat == null)
        {
            Debug.LogWarning("Cat情報が見つかりませんでした。");
            return false;
        }

        // CupTypeを取得（親オブジェクトから）
        Cup cup = GetComponentInParent<Cup>();
        if (cup == null)
        {
            Debug.LogWarning("Cup情報が見つかりませんでした。");
            return false;
        }

        // 組み合わせを判定
        bool result = combinationManager.IsValidCombination(cat.catType, cup.cupType);
        Debug.Log(result ? $"正しい組み合わせ: {cat.catType} と {cup.cupType}" : $"不正な組み合わせ: {cat.catType} と {cup.cupType}");
        return result;
    }

    /// <summary>
    /// 正しい組み合わせの場合の処理
    /// </summary>
    private void HandleCorrectCombination(Collider other)
    {
        Debug.Log("正しい組み合わせでカップイン！");

        SkinnedMeshRenderer characterRenderer = other.GetComponentInChildren<SkinnedMeshRenderer>();
        Material characterMaterial = characterRenderer != null ? characterRenderer.material : null;
        float characterYRotation = other.transform.eulerAngles.y;

        // 非同期でテクスチャをロードし、処理を進める
        if (characterMaterial != null)
        {
            string matName = characterMaterial.name.Replace("(Instance)", "").Trim();
            string[] parts = matName.Split('_');
            string catType = (parts.Length > 1) ? parts[1] : "Default";

            string addressableKey = catType + "_Cupin";
            Addressables.LoadAssetAsync<Texture>(addressableKey).Completed += handle =>
            {
                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    // テクスチャをマテリアルに適用
                    Texture cupinTexture = handle.Result;
                    cupInMaterial.mainTexture = cupinTexture;
                }
                else
                {
                    Debug.LogWarning($"テクスチャ {addressableKey} のロードに失敗しました。");
                }

                // SkinnedMeshRendererを有効化
                if (skinnedMeshRenderer != null)
                {
                    skinnedMeshRenderer.enabled = true;
                    skinnedMeshRenderer.material = cupInMaterial;
                }

                // アニメーションを有効化
                if (animator != null)
                {
                    animator.enabled = true;
                }

                // CatInCupの回転を設定
                CatInCup.transform.rotation = Quaternion.Euler(
                    CatInCup.transform.eulerAngles.x,
                    characterYRotation,
                    CatInCup.transform.eulerAngles.z
                );

                // FXエフェクトを生成して再生
                foreach (var fxPrefab in fxPrefabs)
                {
                    if (fxPrefab != null)
                    {
                        GameObject fxInstance = Instantiate(fxPrefab, CatInCup.transform);
                        ParticleSystem fxParticleSystem = fxInstance.GetComponent<ParticleSystem>();
                        if (fxParticleSystem != null)
                        {
                            fxParticleSystem.Play();
                            if (!fxParticleSystem.main.loop)
                            {
                                Destroy(fxInstance, fxParticleSystem.main.duration);
                            }
                        }
                    }
                }

                // SEを再生
                foreach (var se in seAudioSources)
                {
                    if (se != null)
                    {
                        se.Play();
                    }
                }

                // キャラクターオブジェクトを破棄
                Destroy(other.gameObject);
            };
        }
    }

    /// <summary>
    /// 不正な組み合わせの場合の処理
    /// </summary>
    private void HandleIncorrectCombination()
    {
        Debug.Log("不正解！");

        if (incorrectSe != null) incorrectSe.Play();
        if (incorrectFxPrefab != null)
        {
            GameObject fxInstance = Instantiate(incorrectFxPrefab, transform.position, Quaternion.identity);
            Destroy(fxInstance, 2f); // エフェクトを2秒後に破棄
        }
    }
}
