using UnityEngine;
using UnityEngine.AddressableAssets;

public class ColliderHandler : MonoBehaviour
{
    [SerializeField] private GameObject CatInCup; // カップの中に表示する猫オブジェクト
    [SerializeField] private Material cupInMaterial; // カップイン時の共通マテリアル
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;
    private bool hasTriggered = false;

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
        // 既に処理済みの場合は何もしない
        if (hasTriggered) return;

        // Characterタグが付いているオブジェクトとの衝突を検出
        if (other.CompareTag("Character"))
        {
            hasTriggered = true;

            // 衝突したCharacterのSkinnedMeshRendererとマテリアルを取得
            SkinnedMeshRenderer characterRenderer = other.GetComponentInChildren<SkinnedMeshRenderer>();
            Material characterMaterial = characterRenderer != null ? characterRenderer.material : null;

            // 衝突したCharacterのY軸のRotationを取得
            float characterYRotation = other.transform.eulerAngles.y;

            // CatInCupの出現処理
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.enabled = true;

                // マテリアル名から種類を特定し、対応するテクスチャをロード
                if (characterMaterial != null)
                {
                    string matName = characterMaterial.name.Replace("(Instance)", "").Trim();
                    string[] parts = matName.Split('_');
                    string catType = (parts.Length > 1) ? parts[1] : "Default";

                    // Addressablesを使用して対応するテクスチャをロード
                    string addressableKey = catType + "_Cupin";
                    Addressables.LoadAssetAsync<Texture>(addressableKey).Completed += handle =>
                    {
                        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                        {
                            Texture cupinTexture = handle.Result;
                            cupInMaterial.mainTexture = cupinTexture;
                            skinnedMeshRenderer.material = cupInMaterial;
                        }
                        else
                        {
                            Debug.LogWarning($"テクスチャ {addressableKey} のロードに失敗しました。");
                            skinnedMeshRenderer.material = cupInMaterial;
                        }
                    };
                }
                else
                {
                    skinnedMeshRenderer.material = cupInMaterial;
                }
            }

            // CatInCupのY軸のRotationを設定
            CatInCup.transform.rotation = Quaternion.Euler(
                CatInCup.transform.eulerAngles.x,
                characterYRotation,
                CatInCup.transform.eulerAngles.z
            );

            // CatInCupのAnimatorを有効化
            if (animator != null)
            {
                animator.enabled = true;
            }

            // 衝突した元のCharacterオブジェクトを破棄
            Destroy(other.gameObject);
        }
    }
}
