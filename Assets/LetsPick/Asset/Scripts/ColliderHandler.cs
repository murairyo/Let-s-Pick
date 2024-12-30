using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class ColliderHandler : MonoBehaviour
{
    [SerializeField] private GameObject CatInCup; // カップの中に表示する猫オブジェクト
    [SerializeField] private Material cupInMaterial; // カップイン時の共通マテリアル
    [SerializeField] private List<GameObject> fxPrefabs; // FXのプレファブリスト
    [SerializeField] private List<AudioSource> seAudioSources; // 正解時のSE

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;
    private bool isLocked = false; // 一度成功したらロック

    // カップイン成功時の信号イベント
    public event System.Action OnCupFilled;

    void Start()
    {
        // CatInCupのSkinnedMeshRendererとAnimatorを取得
        skinnedMeshRenderer = CatInCup.GetComponentInChildren<SkinnedMeshRenderer>();
        animator = CatInCup.GetComponent<Animator>();

        // CatInCupを非表示にする
        if (skinnedMeshRenderer != null) skinnedMeshRenderer.enabled = false;
        if (animator != null) animator.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isLocked) return;

        // Characterタグが付いているオブジェクトとの衝突を検出
        if (other.CompareTag("Character"))
        {
            isLocked = true; // ロック

            // カップイン成功時の処理
            Debug.Log("カップイン成功！");
            HandleCupInSuccess(other);

            // 信号を送信
            OnCupFilled?.Invoke();
        }
    }

    private void HandleCupInSuccess(Collider other)
    {
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
                    Texture cupinTexture = handle.Result;
                    cupInMaterial.mainTexture = cupinTexture;
                }

                // スキンメッシュレンダラーを有効化
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
                        var fxParticleSystem = fxInstance.GetComponent<ParticleSystem>();
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

                // 衝突したCharacterオブジェクトを破棄
                Destroy(other.gameObject);
            };
        }
    }
}
