using UnityEngine;

public class AnimatorBooleanController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private GameObject targetObject; // アクティブ化を監視するオブジェクト
    [SerializeField] private Animator animator; // アニメーションを制御するAnimator
    [SerializeField] private Animator secondaryAnimator; // 非アクティブ化する別のアニメーター

    [Header("Animator Parameters")]
    [SerializeField] private string booleanParameter = "Stage01_isActive"; // アニメーションブールのパラメータ名

    private bool previousState = false; // 前回のアクティブ状態を保存

    private void Update()
    {
        bool currentState = targetObject.activeSelf;
        if (currentState != previousState)
        {
            SetAnimatorBoolean(currentState);
            if (!currentState)
            {
                DeactivateSecondaryAnimator();
            }
            previousState = currentState;
        }
    }

    private void SetAnimatorBoolean(bool value)
    {
        if (animator != null && !string.IsNullOrEmpty(booleanParameter))
        {
            animator.SetBool(booleanParameter, value);
            Debug.Log($"Animatorパラメータ '{booleanParameter}' が {value} に設定されました。");
        }
        else
        {
            Debug.LogError("Animatorまたはパラメータ名が正しく設定されていません。");
        }
    }

    private void DeactivateSecondaryAnimator()
    {
        if (secondaryAnimator != null)
        {
            secondaryAnimator.gameObject.SetActive(false);
            Debug.Log("Secondary Animatorが非アクティブ化されました。");
        }
    }
}
