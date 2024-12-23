using UnityEngine;

public class CharacterReaction : MonoBehaviour
{
    // エフェクトとSEを管理する配列
    [Header("エフェクト設定")]
    [SerializeField] private ParticleSystem[] effects; // ParticleSystemの配列
    [Header("SE設定")]
    [SerializeField] private AudioSource[] audioSources; // AudioSourceの配列

    // 反応ごとのID（0:攻撃、1:回避、2:ダメージ など）
    public void PlayReaction(int reactionID)
    {
        // 範囲チェック
        if (reactionID < 0 || reactionID >= effects.Length || reactionID >= audioSources.Length)
        {
            Debug.LogWarning("Invalid reactionID: " + reactionID);
            return;
        }

        // エフェクトを再生
        if (effects[reactionID] != null)
        {
            effects[reactionID].Play();
            Debug.Log("Effect played: " + effects[reactionID].name);
        }

        // SEを再生
        if (audioSources[reactionID] != null)
        {
            audioSources[reactionID].Play();
            Debug.Log("SE played: " + audioSources[reactionID].clip.name);
        }
    }
}
