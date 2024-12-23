using UnityEngine;
using UnityEngine.AddressableAssets;

public class ColliderHandler : MonoBehaviour
{
    [SerializeField] private GameObject CatInCup; // �J�b�v�̒��ɕ\������L�I�u�W�F�N�g
    [SerializeField] private Material cupInMaterial; // �J�b�v�C�����̋��ʃ}�e���A��
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;
    private bool hasTriggered = false;

    void Start()
    {
        // CatInCup��SkinnedMeshRenderer��Animator���擾
        skinnedMeshRenderer = CatInCup.GetComponentInChildren<SkinnedMeshRenderer>();
        animator = CatInCup.GetComponent<Animator>();

        // CatInCup���\���ɂ���
        if (skinnedMeshRenderer != null) skinnedMeshRenderer.enabled = false;
        if (animator != null) animator.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // ���ɏ����ς݂̏ꍇ�͉������Ȃ�
        if (hasTriggered) return;

        // Character�^�O���t���Ă���I�u�W�F�N�g�Ƃ̏Փ˂����o
        if (other.CompareTag("Character"))
        {
            hasTriggered = true;

            // �Փ˂���Character��SkinnedMeshRenderer�ƃ}�e���A�����擾
            SkinnedMeshRenderer characterRenderer = other.GetComponentInChildren<SkinnedMeshRenderer>();
            Material characterMaterial = characterRenderer != null ? characterRenderer.material : null;

            // �Փ˂���Character��Y����Rotation���擾
            float characterYRotation = other.transform.eulerAngles.y;

            // CatInCup�̏o������
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.enabled = true;

                // �}�e���A���������ނ���肵�A�Ή�����e�N�X�`�������[�h
                if (characterMaterial != null)
                {
                    string matName = characterMaterial.name.Replace("(Instance)", "").Trim();
                    string[] parts = matName.Split('_');
                    string catType = (parts.Length > 1) ? parts[1] : "Default";

                    // Addressables���g�p���đΉ�����e�N�X�`�������[�h
                    string addressableKey = catType + "_Cupin";
                    Addressables.LoadAssetAsync<Texture>(addressableKey).Completed += handle =>
                    {
                        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                        {
                            Texture cupinTexture = handle.Result;
                            cupInMaterial.mainTexture = cupinTexture;
                            skinnedMeshRenderer.material = cupInMaterial;
                        }
                        else
                        {
                            Debug.LogWarning($"�e�N�X�`�� {addressableKey} �̃��[�h�Ɏ��s���܂����B");
                            skinnedMeshRenderer.material = cupInMaterial;
                        }
                    };
                }
                else
                {
                    skinnedMeshRenderer.material = cupInMaterial;
                }
            }

            // CatInCup��Y����Rotation��ݒ�
            CatInCup.transform.rotation = Quaternion.Euler(
                CatInCup.transform.eulerAngles.x,
                characterYRotation,
                CatInCup.transform.eulerAngles.z
            );

            // CatInCup��Animator��L����
            if (animator != null)
            {
                animator.enabled = true;
            }

            // �Փ˂�������Character�I�u�W�F�N�g��j��
            Destroy(other.gameObject);
        }
    }
}
