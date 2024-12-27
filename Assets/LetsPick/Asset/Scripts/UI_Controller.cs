using UnityEngine;
using UnityEngine.UI; // UI���������߂ɕK�v

public class UI_Controller : MonoBehaviour
{
    [Header("UI�ݒ�")]
    [SerializeField] private Image targetImage; // Alpha���Ď�����UI��Image

    [Header("Animator�ݒ�")]
    [SerializeField] private Animator targetAnimator; // Alpha 0���ɗL�����E����������Animator
    [SerializeField] private Animator secondaryAnimator; // Alpha 255���ɖ���������Animator
    [SerializeField] private Animator tertiaryAnimator; // ����̃I�u�W�F�N�g���A�N�e�B�u�����ꂽ��L��������Animator

    [Header("����I�u�W�F�N�g�ݒ�")]
    [SerializeField] private GameObject targetObject; // �A�N�e�B�u��Ԃ��Ď�����I�u�W�F�N�g

    [Header("Alpha�̂������l")]
    [SerializeField] private float alphaThreshold = 0.01f; // �����x0�Ƃ݂Ȃ��l�i�����Ȍ덷�����e�j

    void Update()
    {
        if (targetImage == null || targetAnimator == null || secondaryAnimator == null || tertiaryAnimator == null || targetObject == null)
        {
            Debug.LogWarning("�K�v�ȃR���|�[�l���g���ݒ肳��Ă��܂���B");
            return;
        }

        // ���݂�Alpha�l���擾
        float currentAlpha = targetImage.color.a;

        // Alpha�l��0�ɋ߂��ꍇ�AtargetAnimator�𖳌���
        if (currentAlpha <= alphaThreshold)
        {
            if (targetAnimator.enabled) // ���ɖ���������Ă��Ȃ��ꍇ
            {
                targetAnimator.enabled = false;
                Debug.Log("targetAnimator������������܂����B");
            }
        }
        // Alpha�l��255�ɋ߂��ꍇ�AtargetAnimator��L�������AsecondaryAnimator�𖳌���
        else if (currentAlpha >= 1.0f - alphaThreshold)
        {
            if (!targetAnimator.enabled) // ���ɗL��������Ă��Ȃ��ꍇ
            {
                targetAnimator.enabled = true;
                Debug.Log("targetAnimator���L��������܂����B");
            }

            if (secondaryAnimator.enabled) // secondaryAnimator���L��������Ă���ꍇ
            {
                secondaryAnimator.enabled = false;
                Debug.Log("secondaryAnimator������������܂����B");
            }
        }
        // Alpha��0�ł�255�ł��Ȃ��ꍇ�AsecondaryAnimator��L����
        else
        {
            if (!secondaryAnimator.enabled)
            {
                secondaryAnimator.enabled = true;
                Debug.Log("secondaryAnimator���L��������܂����B");
            }
        }

        // ����̃I�u�W�F�N�g�̃A�N�e�B�u��Ԃ��Ď�
        if (targetObject.activeSelf)
        {
            // targetObject���A�N�e�B�u�����ꂽ��targetAnimator�𖳌���
            if (targetAnimator.enabled)
            {
                targetAnimator.enabled = false;
                Debug.Log("targetObject���A�N�e�B�u�����ꂽ���߁AtargetAnimator������������܂����B");
            }

            // targetObject���A�N�e�B�u�����ꂽ��tertiaryAnimator��L����
            if (!tertiaryAnimator.enabled)
            {
                tertiaryAnimator.enabled = true;
                Debug.Log("targetObject���A�N�e�B�u�����ꂽ���߁AtertiaryAnimator���L��������܂����B");
            }
        }
        else
        {
            // targetObject����A�N�e�B�u�̏ꍇ�AtertiaryAnimator�𖳌���
            if (tertiaryAnimator.enabled)
            {
                tertiaryAnimator.enabled = false;
                Debug.Log("targetObject����A�N�e�B�u�����ꂽ���߁AtertiaryAnimator������������܂����B");
            }
        }
    }
}
