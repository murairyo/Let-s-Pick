using UnityEngine;
using System.Collections;

public class CatAppearanceController : MonoBehaviour
{
    [Header("Textures for Each State")]
    [SerializeField] private Texture waitingTexture;   // �ҋ@���̃e�N�X�`��
    [SerializeField] private Texture grabbedTexture;   // �͂܂ꂽ�Ƃ��̃e�N�X�`��
    [SerializeField] private Texture dropTexture;      // �������̃e�N�X�`��

    [Header("Sound Effects")]
    [SerializeField] private AudioClip grabSE;         // �͂܂ꂽ�Ƃ���SE
    [SerializeField] private AudioClip collisionSE;    // �Փˎ���SE
    [SerializeField] private AudioClip[] waitingSEs;   // �ҋ@���̃����_��SE

    [Header("Effects")]
    [SerializeField] private GameObject grabEffectPrefab;          // �͂܂ꂽ�Ƃ��̃G�t�F�N�g
    [SerializeField] private GameObject collisionImpactEffectPrefab; // �Ռ�FX
    [SerializeField] private GameObject collisionConfusionEffectPrefab; // ����FX
    [SerializeField] private GameObject waitingEffectPrefab;       // �ҋ@���̃G�t�F�N�g

    [Header("Waiting Sound Settings")]
    [SerializeField] private float minWaitTime = 3f;   // �ҋ@SE�Đ��̍ŏ��Ԋu
    [SerializeField] private float maxWaitTime = 7f;   // �ҋ@SE�Đ��̍ő�Ԋu
    [SerializeField] private float waitingSEProbability = 0.5f; // SE�Đ��m���i0�`1�j

    private Material catMaterial;      // �L�̃}�e���A��
    private Animator animator;         // �L�̃A�j���[�V��������
    private AudioSource audioSource;   // �����Đ��p��AudioSource

    private bool isGrabbed = false;    // �͂܂ꂽ��Ԃ��ǂ���
    private int grabCount = 0;         // �͂܂ꂽ��
    private GameObject waitingEffectInstance; // �ҋ@�G�t�F�N�g�̃C���X�^���X

    // ���O����^�O���X�g
    private readonly string[] ignoredTags = { "Cup", "Bot_Collider" };

    private void Awake()
    {
        // �R���|�[�l���g���擾
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // �L���f���̃}�e���A�����擾
        SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            catMaterial = smr.material;
        }
        else
        {
            Debug.LogWarning("SkinnedMeshRenderer��������܂���ł����B");
        }

        // ������Ԃ��X�V
        UpdateTexture();
        StartCoroutine(PlayRandomWaitingSE()); // �����_���ȑҋ@SE�Đ����J�n
        SpawnWaitingEffect();                 // �ҋ@�G�t�F�N�g�𐶐�
    }

    // �͂܂ꂽ�Ƃ��̏���
    public void OnSelect()
    {
        isGrabbed = true;    // �͂܂ꂽ��ԂɕύX
        grabCount++;         // �͂܂ꂽ�񐔂𑝉�
        UpdateTexture();     // �e�N�X�`�����X�V

        // �A�j���[�V������ύX
        animator.SetBool("IsGrabbed", true);

        // �͂܂ꂽ�Ƃ���SE�ƃG�t�F�N�g���Đ�
        PlaySound(grabSE);
        SpawnEffect(grabEffectPrefab);

        // �ҋ@�G�t�F�N�g���폜
        if (waitingEffectInstance != null)
        {
            Destroy(waitingEffectInstance);
            waitingEffectInstance = null;
        }
    }

    // �͂܂ꂽ��Ԃ��������ꂽ�Ƃ��̏���
    public void OnUnselect()
    {
        isGrabbed = false; // �͂܂�Ă��Ȃ���ԂɕύX
        UpdateTexture();   // �e�N�X�`�����X�V

        // �A�j���[�V���������Z�b�g
        animator.SetBool("IsGrabbed", false);
    }

    // �Փˎ��̏���
    private void OnCollisionEnter(Collision collision)
    {
        // ���O����^�O�ł���Ώ����𒆒f
        foreach (string tag in ignoredTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                return;
            }
        }

        // �͂܂�Ă��Ȃ��A���͂܂ꂽ�񐔂�1�ȏ�̏ꍇ�̂ݏ���
        if (!isGrabbed && grabCount > 0)
        {
            // �Փˎ���FX��SE�𔭐�
            SpawnEffect(collisionConfusionEffectPrefab, transform, true); // ����FX
            SpawnEffect(collisionImpactEffectPrefab, collision.contacts[0].point, Vector3.up); // �Ռ�FX�i���������j
            PlaySound(collisionSE);
        }
    }

    // ���݂̏�Ԃɉ����ăe�N�X�`�����X�V
    private void UpdateTexture()
    {
        if (catMaterial == null) return;

        if (isGrabbed)
        {
            catMaterial.mainTexture = grabbedTexture;
        }
        else
        {
            catMaterial.mainTexture = waitingTexture;
        }
    }

    // AudioClip���Đ�
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // ��x�����Đ�
        }
    }

    // �����_���ɑҋ@SE���Đ�����R���[�`��
    private IEnumerator PlayRandomWaitingSE()
    {
        while (true)
        {
            // �����_���Ȏ��Ԃ�ҋ@
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // �m���Ɋ�Â���SE���Đ�
            if (Random.value < waitingSEProbability && waitingSEs.Length > 0)
            {
                AudioClip clip = waitingSEs[Random.Range(0, waitingSEs.Length)];
                PlaySound(clip);
            }
        }
    }

    // �w�肵���G�t�F�N�g�𐶐��i�I�v�V�����Őe�̉�]�ɏ]���j
    private void SpawnEffect(GameObject effectPrefab, Transform parent = null, bool attachToParent = false)
    {
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, parent?.position ?? transform.position, parent?.rotation ?? Quaternion.identity);
            if (attachToParent && parent != null)
            {
                effect.transform.SetParent(parent);
            }
            Destroy(effect, 5f); // �Đ���ɍ폜
        }
    }

    // �Ռ�FX�𐂒������ɐ���
    private void SpawnEffect(GameObject effectPrefab, Vector3 position, Vector3 normal)
    {
        if (effectPrefab != null)
        {
            Quaternion rotation = Quaternion.identity; // ��]�𐂒������ɌŒ�
            GameObject effect = Instantiate(effectPrefab, position, rotation);
            Destroy(effect, 5f); // �Đ���ɍ폜
        }
    }

    // �ҋ@�G�t�F�N�g�𐶐�
    private void SpawnWaitingEffect()
    {
        if (waitingEffectPrefab != null)
        {
            waitingEffectInstance = Instantiate(waitingEffectPrefab, transform.position, transform.rotation, transform);
        }
    }
}
