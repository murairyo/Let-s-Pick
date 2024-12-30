using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class ColliderHandlerStage3 : MonoBehaviour
{
    [SerializeField] private GameObject CatInCup; // �J�b�v�ɕ\�������L
    [SerializeField] private Material cupInMaterial; // �J�b�v�C�����̋��ʃ}�e���A��
    [SerializeField] private List<GameObject> fxPrefabs; // ��������FX�v���t�@�u���X�g
    [SerializeField] private List<AudioSource> seAudioSources; // ��������SE
    [SerializeField] private GameObject incorrectFxPrefab; // �s�������̃G�t�F�N�g
    [SerializeField] private AudioSource incorrectSe; // �s�������̉���

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Animator animator;
    private bool isLocked = false; // ��x���������珈�������b�N����

    public event System.Action OnCupFilled; // �J�b�v�C���������̃C�x���g

    private CombinationManager combinationManager; // CombinationManager�̎Q��

    void Start()
    {
        // CatInCup�̃R���|�[�l���g���擾
        skinnedMeshRenderer = CatInCup.GetComponentInChildren<SkinnedMeshRenderer>();
        animator = CatInCup.GetComponent<Animator>();

        // CatInCup���\���ɂ���
        if (skinnedMeshRenderer != null) skinnedMeshRenderer.enabled = false;
        if (animator != null) animator.enabled = false;

        // CombinationManager���擾
        combinationManager = FindObjectOfType<CombinationManager>();
        if (combinationManager == null)
        {
            Debug.LogError("CombinationManager��������܂���I�V�[���ɔz�u���Ă��������B");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isLocked) return; // ���Ƀ��b�N����Ă���ꍇ�͏������s��Ȃ�

        if (other.CompareTag("Character"))
        {
            // �g�ݍ��킹���m�F
            bool isCorrect = CheckCombination(other);

            if (isCorrect)
            {
                isLocked = true; // �������Ƀ��b�N
                HandleCorrectCombination(other);
                OnCupFilled?.Invoke(); // �����C�x���g�𔭉�
            }
            else
            {
                HandleIncorrectCombination();
            }
        }
    }

    /// <summary>
    /// �g�ݍ��킹���m�F����
    /// </summary>
    private bool CheckCombination(Collider other)
    {
        // CatType���擾
        Cat cat = other.GetComponent<Cat>();
        if (cat == null)
        {
            Debug.LogWarning("Cat��񂪌�����܂���ł����B");
            return false;
        }

        // CupType���擾�i�e�I�u�W�F�N�g����j
        Cup cup = GetComponentInParent<Cup>();
        if (cup == null)
        {
            Debug.LogWarning("Cup��񂪌�����܂���ł����B");
            return false;
        }

        // �g�ݍ��킹�𔻒�
        bool result = combinationManager.IsValidCombination(cat.catType, cup.cupType);
        Debug.Log(result ? $"�������g�ݍ��킹: {cat.catType} �� {cup.cupType}" : $"�s���ȑg�ݍ��킹: {cat.catType} �� {cup.cupType}");
        return result;
    }

    /// <summary>
    /// �������g�ݍ��킹�̏ꍇ�̏���
    /// </summary>
    private void HandleCorrectCombination(Collider other)
    {
        Debug.Log("�������g�ݍ��킹�ŃJ�b�v�C���I");

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

                // SkinnedMeshRenderer��L����
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

                // �L�����N�^�[�I�u�W�F�N�g��j��
                Destroy(other.gameObject);
            };
        }
    }

    /// <summary>
    /// �s���ȑg�ݍ��킹�̏ꍇ�̏���
    /// </summary>
    private void HandleIncorrectCombination()
    {
        Debug.Log("�s�����I");

        if (incorrectSe != null) incorrectSe.Play();
        if (incorrectFxPrefab != null)
        {
            GameObject fxInstance = Instantiate(incorrectFxPrefab, transform.position, Quaternion.identity);
            Destroy(fxInstance, 2f); // �G�t�F�N�g��2�b��ɔj��
        }
    }
}
