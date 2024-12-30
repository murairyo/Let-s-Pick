using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    /// <summary>
    /// シーンをリロードする
    /// </summary>
    public void WhenSelectorHovered()
    {
        // 現在のシーンを再読み込み
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Debug.Log("Scene reloaded: " + currentScene.name);
    }
}
