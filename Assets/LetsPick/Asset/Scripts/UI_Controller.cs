using UnityEngine;
using UnityEngine.UI; // UIを扱うために必要

public class UI_Controller : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private Image targetImage; // Alphaを監視するUIのImage

    [Header("Animator設定")]
    [SerializeField] private Animator targetAnimator; // Alpha 0時に有効化・無効化するAnimator
    [SerializeField] private Animator secondaryAnimator; // Alpha 255時に無効化するAnimator
    [SerializeField] private Animator tertiaryAnimator; // 特定のオブジェクトがアクティブ化されたら有効化するAnimator

    [Header("特定オブジェクト設定")]
    [SerializeField] private GameObject targetObject; // アクティブ状態を監視するオブジェクト

    [Header("Alphaのしきい値")]
    [SerializeField] private float alphaThreshold = 0.01f; // 透明度0とみなす値（小さな誤差を許容）

    void Update()
    {
        if (targetImage == null || targetAnimator == null || secondaryAnimator == null || tertiaryAnimator == null || targetObject == null)
        {
            Debug.LogWarning("必要なコンポーネントが設定されていません。");
            return;
        }

        // 現在のAlpha値を取得
        float currentAlpha = targetImage.color.a;

        // Alpha値が0に近い場合、targetAnimatorを無効化
        if (currentAlpha <= alphaThreshold)
        {
            if (targetAnimator.enabled) // 既に無効化されていない場合
            {
                targetAnimator.enabled = false;
                Debug.Log("targetAnimatorが無効化されました。");
            }
        }
        // Alpha値が255に近い場合、targetAnimatorを有効化し、secondaryAnimatorを無効化
        else if (currentAlpha >= 1.0f - alphaThreshold)
        {
            if (!targetAnimator.enabled) // 既に有効化されていない場合
            {
                targetAnimator.enabled = true;
                Debug.Log("targetAnimatorが有効化されました。");
            }

            if (secondaryAnimator.enabled) // secondaryAnimatorが有効化されている場合
            {
                secondaryAnimator.enabled = false;
                Debug.Log("secondaryAnimatorが無効化されました。");
            }
        }
        // Alphaが0でも255でもない場合、secondaryAnimatorを有効化
        else
        {
            if (!secondaryAnimator.enabled)
            {
                secondaryAnimator.enabled = true;
                Debug.Log("secondaryAnimatorが有効化されました。");
            }
        }

        // 特定のオブジェクトのアクティブ状態を監視
        if (targetObject.activeSelf)
        {
            // targetObjectがアクティブ化されたらtargetAnimatorを無効化
            if (targetAnimator.enabled)
            {
                targetAnimator.enabled = false;
                Debug.Log("targetObjectがアクティブ化されたため、targetAnimatorが無効化されました。");
            }

            // targetObjectがアクティブ化されたらtertiaryAnimatorを有効化
            if (!tertiaryAnimator.enabled)
            {
                tertiaryAnimator.enabled = true;
                Debug.Log("targetObjectがアクティブ化されたため、tertiaryAnimatorが有効化されました。");
            }
        }
        else
        {
            // targetObjectが非アクティブの場合、tertiaryAnimatorを無効化
            if (tertiaryAnimator.enabled)
            {
                tertiaryAnimator.enabled = false;
                Debug.Log("targetObjectが非アクティブ化されたため、tertiaryAnimatorが無効化されました。");
            }
        }
    }
}
