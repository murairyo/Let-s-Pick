using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // シーン名を指定する変数
    [SerializeField] private string sceneName;

    // シーンをロードする関数
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("シーン名が設定されていません。インスペクターでシーン名を指定してください。");
        }
    }
}
