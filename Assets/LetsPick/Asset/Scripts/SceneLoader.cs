using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // �V�[�������w�肷��ϐ�
    [SerializeField] private string sceneName;

    // �V�[�������[�h����֐�
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("�V�[�������ݒ肳��Ă��܂���B�C���X�y�N�^�[�ŃV�[�������w�肵�Ă��������B");
        }
    }
}
