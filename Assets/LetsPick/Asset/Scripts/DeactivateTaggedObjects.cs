using UnityEngine;

public class DeactivateTaggedObjects : MonoBehaviour
{
    // �{�^���ŌĂяo�����֐�
    public void DeactivateActiveObjectsWithTag()
    {
        // "Cup_SideCollider" �^�O�̂������ׂẴI�u�W�F�N�g���擾
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Cup_SideCollider");

        foreach (GameObject obj in objectsWithTag)
        {
            // �A�N�e�B�u��Ԃ��m�F
            if (obj.activeSelf)
            {
                // ��A�N�e�B�u��
                obj.SetActive(false);
            }
        }
    }
}
