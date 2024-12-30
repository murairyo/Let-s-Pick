using UnityEngine;

public class AnimatorEnableController : MonoBehaviour
{
    // インスペクターで指定するアニメーター
    [SerializeField]
    private Animator animator1;

    [SerializeField]
    private Animator animator2;

    /// <summary>
    /// アニメーター1の有効状態を切り替える (現在の状態と逆にする)
    /// </summary>
    public void ToggleAnimator1Enable()
    {
        if (animator1 != null)
        {
            animator1.enabled = !animator1.enabled;
        }
        else
        {
            Debug.LogWarning("Animator1 is not assigned.");
        }
    }

    /// <summary>
    /// アニメーター2の有効状態を切り替える (現在の状態と逆にする)
    /// </summary>
    public void ToggleAnimator2Enable()
    {
        if (animator2 != null)
        {
            animator2.enabled = !animator2.enabled;
        }
        else
        {
            Debug.LogWarning("Animator2 is not assigned.");
        }
    }

    /// <summary>
    /// このスクリプトがアタッチされているオブジェクトを非アクティブにする
    /// </summary>
    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
        Debug.Log("The attached GameObject has been deactivated.");
    }
}
