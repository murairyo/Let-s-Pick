using UnityEngine;

public class CharacterReaction : MonoBehaviour
{
    // �G�t�F�N�g��SE���Ǘ�����z��
    [Header("�G�t�F�N�g�ݒ�")]
    [SerializeField] private ParticleSystem[] effects; // ParticleSystem�̔z��
    [Header("SE�ݒ�")]
    [SerializeField] private AudioSource[] audioSources; // AudioSource�̔z��

    // �������Ƃ�ID�i0:�U���A1:����A2:�_���[�W �Ȃǁj
    public void PlayReaction(int reactionID)
    {
        // �͈̓`�F�b�N
        if (reactionID < 0 || reactionID >= effects.Length || reactionID >= audioSources.Length)
        {
            Debug.LogWarning("Invalid reactionID: " + reactionID);
            return;
        }

        // �G�t�F�N�g���Đ�
        if (effects[reactionID] != null)
        {
            effects[reactionID].Play();
            Debug.Log("Effect played: " + effects[reactionID].name);
        }

        // SE���Đ�
        if (audioSources[reactionID] != null)
        {
            audioSources[reactionID].Play();
            Debug.Log("SE played: " + audioSources[reactionID].clip.name);
        }
    }
}
