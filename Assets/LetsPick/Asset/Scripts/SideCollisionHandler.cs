using UnityEngine;

public class SideCollisionHandler : MonoBehaviour
{
    [Header("�e����΂��͂̐ݒ�")]
    [SerializeField] private float forceMagnitude = 100f; // �e����΂��͂̑傫��

    private void OnTriggerEnter(Collider other)
    {
        // �Փ˂����I�u�W�F�N�g��Character�^�O�������Ă��邩�`�F�b�N
        if (other.CompareTag("Character"))
        {
            Rigidbody characterRigidbody = other.GetComponent<Rigidbody>();

            if (characterRigidbody != null)
            {
                // �Փ˂̕������v�Z
                Vector3 collisionDirection = (other.transform.position - transform.position).normalized;

                // Rigidbody�ɗ͂�������
                characterRigidbody.AddForce(collisionDirection * forceMagnitude, ForceMode.Impulse);

                Debug.Log($"{other.gameObject.name} �� {gameObject.name} �̃g���K�[�ɓ���A�e����΂���܂����B");
            }
            else
            {
                Debug.LogWarning("Character��Rigidbody���A�^�b�`����Ă��܂���B");
            }
        }
    }
}
