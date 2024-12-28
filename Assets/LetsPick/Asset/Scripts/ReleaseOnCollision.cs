using UnityEngine;
using System.Collections;
using Oculus.Interaction;

public class TemporarilyDisableGrabbable : MonoBehaviour
{
    [Tooltip("�݂͂𐧌䂷��Grabbable�R���|�[�l���g")]
    [SerializeField] private Grabbable grabbable;

    [Tooltip("Grabbable���ăA�N�e�B�u������܂ł̒x�����ԁi�b�j")]
    [SerializeField] private float reenableDelay = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Cup_SideCollider�^�O�ɐG�ꂽ�ꍇ
        if (other.CompareTag("Cup_SideCollider"))
        {
            Debug.Log("Cup_SideCollider�ɐڐG�BGrabbable���ꎞ���������܂��B");

            // Grabbable���ꎞ�I�ɖ�����
            DisableGrabbable();
        }
    }

    private void DisableGrabbable()
    {
        if (grabbable != null)
        {
            // Grabbable���A�N�e�B�u��
            grabbable.enabled = false;
            Debug.Log("Grabbable������������܂����B");

            // �ăA�N�e�B�u����x�����s
            StartCoroutine(ReenableGrabbableAfterDelay());
        }
        else
        {
            Debug.LogWarning("Grabbable���ݒ肳��Ă��܂���B");
        }
    }

    private IEnumerator ReenableGrabbableAfterDelay()
    {
        // �w�肵���x�����Ԃ�ҋ@
        yield return new WaitForSeconds(reenableDelay);

        if (grabbable != null)
        {
            // Grabbable���ĂуA�N�e�B�u��
            grabbable.enabled = true;
            Debug.Log("Grabbable���ĂїL��������܂����B");
        }
    }
}
