using UnityEngine;

public class DeactivateRoot : MonoBehaviour
{
    // �匳�̐e�I�u�W�F�N�g�i���[�g�I�u�W�F�N�g�j���A�N�e�B�u������֐�
    public void DeactivateRootObject()
    {
        // ���[�g�I�u�W�F�N�g���A�N�e�B�u��
        transform.root.gameObject.SetActive(false);
    }
}
