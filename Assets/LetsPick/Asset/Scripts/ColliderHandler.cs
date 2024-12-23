using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

public class ColliderHandler : MonoBehaviour
{
    [SerializeField] private GameObject CatInCup; // �J�b�v�̒��ɕ\������L�I�u�W�F�N�g
    [SerializeField] private Material cupInMaterial; // �J�b�v�C�����̋��ʃ}�e���A��
    [SerializeField] private List<GameObject> fxPrefabs; // FX�̃v���t�@�u���X�g
    [SerializeField] private List<AudioSource> seAudioSources; // ������SE���Ǘ�

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

            // Character�����擾
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
                        // �e�N�X�`�����}�e���A���ɓK�p
                        Texture cupinTexture = handle.Result;
                        cupInMaterial.mainTexture = cupinTexture;
                    }
                    else
                    {
                        Debug.LogWarning($"�e�N�X�`�� {addressableKey} �̃��[�h�Ɏ��s���܂����B");
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
                            ParticleSystem fxParticleSystem = fxInstance.GetComponent<ParticleSystem>();
                            if (fxParticleSystem != null)
                            {
                                fxParticleSystem.Play();

                                // ���[�v�ݒ���m�F���Ĕj��������
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
            else
            {
                Debug.LogWarning("Character�̃}�e���A����������܂���ł����B");
            }
        }
    }
}
