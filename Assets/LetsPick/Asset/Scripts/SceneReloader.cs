using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    /// <summary>
    /// �V�[���������[�h����
    /// </summary>
    public void WhenSelectorHovered()
    {
        // ���݂̃V�[�����ēǂݍ���
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Debug.Log("Scene reloaded: " + currentScene.name);
    }
}
