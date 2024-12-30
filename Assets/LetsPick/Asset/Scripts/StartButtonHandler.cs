using UnityEngine;

public class StartButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject stage1; // ステージ1のオブジェクト
    [SerializeField] private StageManager stageManager; // StageManagerの参照

    public void ActivateStage1()
    {
        if (stage1 != null)
        {
            stage1.SetActive(true); // ステージ1をアクティブ化
        }

        if (stageManager != null)
        {
            stageManager.InitializeFirstStage(); // ステージ1の初期化
        }
    }
}
