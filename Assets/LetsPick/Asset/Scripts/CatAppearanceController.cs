using UnityEngine;
using System.Collections;

public class CatAppearanceController : MonoBehaviour
{
    [Header("Textures for Each State")]
    [SerializeField] private Texture waitingTexture;   // 待機中のテクスチャ
    [SerializeField] private Texture grabbedTexture;   // 掴まれたときのテクスチャ
    [SerializeField] private Texture dropTexture;      // 落下中のテクスチャ

    [Header("Sound Effects")]
    [SerializeField] private AudioClip grabSE;         // 掴まれたときのSE
    [SerializeField] private AudioClip collisionSE;    // 衝突時のSE
    [SerializeField] private AudioClip[] waitingSEs;   // 待機中のランダムSE

    [Header("Effects")]
    [SerializeField] private GameObject grabEffectPrefab;          // 掴まれたときのエフェクト
    [SerializeField] private GameObject collisionImpactEffectPrefab; // 衝撃FX
    [SerializeField] private GameObject collisionConfusionEffectPrefab; // 混乱FX
    [SerializeField] private GameObject waitingEffectPrefab;       // 待機中のエフェクト

    [Header("Waiting Sound Settings")]
    [SerializeField] private float minWaitTime = 3f;   // 待機SE再生の最小間隔
    [SerializeField] private float maxWaitTime = 7f;   // 待機SE再生の最大間隔
    [SerializeField] private float waitingSEProbability = 0.5f; // SE再生確率（0～1）

    private Material catMaterial;      // 猫のマテリアル
    private Animator animator;         // 猫のアニメーション制御
    private AudioSource audioSource;   // 音声再生用のAudioSource

    private bool isGrabbed = false;    // 掴まれた状態かどうか
    private int grabCount = 0;         // 掴まれた回数
    private GameObject waitingEffectInstance; // 待機エフェクトのインスタンス

    // 除外するタグリスト
    private readonly string[] ignoredTags = { "Cup", "Bot_Collider" };

    private void Awake()
    {
        // コンポーネントを取得
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // 猫モデルのマテリアルを取得
        SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            catMaterial = smr.material;
        }
        else
        {
            Debug.LogWarning("SkinnedMeshRendererが見つかりませんでした。");
        }

        // 初期状態を更新
        UpdateTexture();
        StartCoroutine(PlayRandomWaitingSE()); // ランダムな待機SE再生を開始
        SpawnWaitingEffect();                 // 待機エフェクトを生成
    }

    // 掴まれたときの処理
    public void OnSelect()
    {
        isGrabbed = true;    // 掴まれた状態に変更
        grabCount++;         // 掴まれた回数を増加
        UpdateTexture();     // テクスチャを更新

        // アニメーションを変更
        animator.SetBool("IsGrabbed", true);

        // 掴まれたときのSEとエフェクトを再生
        PlaySound(grabSE);
        SpawnEffect(grabEffectPrefab);

        // 待機エフェクトを削除
        if (waitingEffectInstance != null)
        {
            Destroy(waitingEffectInstance);
            waitingEffectInstance = null;
        }
    }

    // 掴まれた状態が解除されたときの処理
    public void OnUnselect()
    {
        isGrabbed = false; // 掴まれていない状態に変更
        UpdateTexture();   // テクスチャを更新

        // アニメーションをリセット
        animator.SetBool("IsGrabbed", false);
    }

    // 衝突時の処理
    private void OnCollisionEnter(Collision collision)
    {
        // 除外するタグであれば処理を中断
        foreach (string tag in ignoredTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                return;
            }
        }

        // 掴まれていない、かつ掴まれた回数が1以上の場合のみ処理
        if (!isGrabbed && grabCount > 0)
        {
            // 衝突時のFXとSEを発生
            SpawnEffect(collisionConfusionEffectPrefab, transform, true); // 混乱FX
            SpawnEffect(collisionImpactEffectPrefab, collision.contacts[0].point, Vector3.up); // 衝撃FX（垂直方向）
            PlaySound(collisionSE);
        }
    }

    // 現在の状態に応じてテクスチャを更新
    private void UpdateTexture()
    {
        if (catMaterial == null) return;

        if (isGrabbed)
        {
            catMaterial.mainTexture = grabbedTexture;
        }
        else
        {
            catMaterial.mainTexture = waitingTexture;
        }
    }

    // AudioClipを再生
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // 一度だけ再生
        }
    }

    // ランダムに待機SEを再生するコルーチン
    private IEnumerator PlayRandomWaitingSE()
    {
        while (true)
        {
            // ランダムな時間を待機
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // 確率に基づいてSEを再生
            if (Random.value < waitingSEProbability && waitingSEs.Length > 0)
            {
                AudioClip clip = waitingSEs[Random.Range(0, waitingSEs.Length)];
                PlaySound(clip);
            }
        }
    }

    // 指定したエフェクトを生成（オプションで親の回転に従う）
    private void SpawnEffect(GameObject effectPrefab, Transform parent = null, bool attachToParent = false)
    {
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, parent?.position ?? transform.position, parent?.rotation ?? Quaternion.identity);
            if (attachToParent && parent != null)
            {
                effect.transform.SetParent(parent);
            }
            Destroy(effect, 5f); // 再生後に削除
        }
    }

    // 衝撃FXを垂直方向に生成
    private void SpawnEffect(GameObject effectPrefab, Vector3 position, Vector3 normal)
    {
        if (effectPrefab != null)
        {
            Quaternion rotation = Quaternion.identity; // 回転を垂直方向に固定
            GameObject effect = Instantiate(effectPrefab, position, rotation);
            Destroy(effect, 5f); // 再生後に削除
        }
    }

    // 待機エフェクトを生成
    private void SpawnWaitingEffect()
    {
        if (waitingEffectPrefab != null)
        {
            waitingEffectInstance = Instantiate(waitingEffectPrefab, transform.position, transform.rotation, transform);
        }
    }
}
