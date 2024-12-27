using UnityEngine;

public class AnimatorBooleanController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private GameObject targetObject; // �A�N�e�B�u�����Ď�����I�u�W�F�N�g
    [SerializeField] private Animator animator; // �A�j���[�V�����𐧌䂷��Animator
    [SerializeField] private Animator secondaryAnimator; // ��A�N�e�B�u������ʂ̃A�j���[�^�[

    [Header("Animator Parameters")]
    [SerializeField] private string booleanParameter = "Stage01_isActive"; // �A�j���[�V�����u�[���̃p�����[�^��

    private bool previousState = false; // �O��̃A�N�e�B�u��Ԃ�ۑ�

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
            Debug.Log($"Animator�p�����[�^ '{booleanParameter}' �� {value} �ɐݒ肳��܂����B");
        }
        else
        {
            Debug.LogError("Animator�܂��̓p�����[�^�����������ݒ肳��Ă��܂���B");
        }
    }

    private void DeactivateSecondaryAnimator()
    {
        if (secondaryAnimator != null)
        {
            secondaryAnimator.gameObject.SetActive(false);
            Debug.Log("Secondary Animator����A�N�e�B�u������܂����B");
        }
    }
}
