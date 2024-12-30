using UnityEngine;

public class StartButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject stage1; // �X�e�[�W1�̃I�u�W�F�N�g
    [SerializeField] private StageManager stageManager; // StageManager�̎Q��

    public void ActivateStage1()
    {
        if (stage1 != null)
        {
            stage1.SetActive(true); // �X�e�[�W1���A�N�e�B�u��
        }

        if (stageManager != null)
        {
            stageManager.InitializeFirstStage(); // �X�e�[�W1�̏�����
        }
    }
}
