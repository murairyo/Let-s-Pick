using UnityEngine;

public class AnimatorEnableController : MonoBehaviour
{
    // �C���X�y�N�^�[�Ŏw�肷��A�j���[�^�[
    [SerializeField]
    private Animator animator1;

    [SerializeField]
    private Animator animator2;

    /// <summary>
    /// �A�j���[�^�[1�̗L����Ԃ�؂�ւ��� (���݂̏�ԂƋt�ɂ���)
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
    /// �A�j���[�^�[2�̗L����Ԃ�؂�ւ��� (���݂̏�ԂƋt�ɂ���)
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
    /// ���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g���A�N�e�B�u�ɂ���
    /// </summary>
    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
        Debug.Log("The attached GameObject has been deactivated.");
    }
}
