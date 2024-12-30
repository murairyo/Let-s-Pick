using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class ColliderHandler : MonoBehaviour
{
    [SerializeField] private GameObject CatInCup; // �J�b�v�̒��ɕ\������L�I�u�W�F�N�g
    [SerializeField] private Material cupInMaterial; // �J�b�v�C�����̋��ʃ}�e���A��
    [SerializeField] private List<GameObject> fxPrefabs; // FX�̃v���t�@�u���X�g
    [SerializeField] private List<AudioSource> seAudioSources; // ��������SE

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;
    private bool isLocked = false; // ��x���������烍�b�N

    // �J�b�v�C���������̐M���C�x���g
    public event System.Action OnCupFilled;

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
        if (isLocked) return;

        // Character�^�O���t���Ă���I�u�W�F�N�g�Ƃ̏Փ˂����o
        if (other.CompareTag("Character"))
        {
            isLocked = true; // ���b�N

            // �J�b�v�C���������̏���
            Debug.Log("�J�b�v�C�������I");
            HandleCupInSuccess(other);

            // �M���𑗐M
            OnCupFilled?.Invoke();
        }
    }

    private void HandleCupInSuccess(Collider other)
    {
        SkinnedMeshRenderer characterRenderer = other.GetComponentInChildren<SkinnedMeshRenderer>();
        Material characterMaterial = characterRenderer != null ? characterRenderer.material : null;
        float characterYRotation = other.transform.eulerAngles.y;

        // �񓯊��Ńe�N�X�`�������[�h���A������i�߂�
        if (characterMaterial != null)
        {
            string matName = characterMaterial.name.Replace("(Instance)", "").Trim();
            string[] parts = matName.Split('_');
            string catType = (parts.Length > 1) ? parts[1] : "Default";

            string addressableKey = catType + "_Cupin";
            Addressables.LoadAssetAsync<Texture>(addressableKey).Completed += handle =>
            {
                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    Texture cupinTexture = handle.Result;
                    cupInMaterial.mainTexture = cupinTexture;
                }

                // �X�L�����b�V�������_���[��L����
                if (skinnedMeshRenderer != null)
                {
                    skinnedMeshRenderer.enabled = true;
                    skinnedMeshRenderer.material = cupInMaterial;
                }

                // �A�j���[�V������L����
                if (animator != null)
                {
                    animator.enabled = true;
                }

                // CatInCup�̉�]��ݒ�
                CatInCup.transform.rotation = Quaternion.Euler(
                    CatInCup.transform.eulerAngles.x,
                    characterYRotation,
                    CatInCup.transform.eulerAngles.z
                );

                // FX�G�t�F�N�g�𐶐����čĐ�
                foreach (var fxPrefab in fxPrefabs)
                {
                    if (fxPrefab != null)
                    {
                        GameObject fxInstance = Instantiate(fxPrefab, CatInCup.transform);
                        var fxParticleSystem = fxInstance.GetComponent<ParticleSystem>();
                        if (fxParticleSystem != null)
                        {
                            fxParticleSystem.Play();
                            if (!fxParticleSystem.main.loop)
                            {
                                Destroy(fxInstance, fxParticleSystem.main.duration);
                            }
                        }
                    }
                }

                // SE���Đ�
                foreach (var se in seAudioSources)
                {
                    if (se != null)
                    {
                        se.Play();
                    }
                }

                // �Փ˂���Character�I�u�W�F�N�g��j��
                Destroy(other.gameObject);
            };
        }
    }
}
