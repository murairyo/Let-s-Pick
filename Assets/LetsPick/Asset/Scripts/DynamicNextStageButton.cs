using UnityEngine;

public class DynamicNextStageButton : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;

    public void OnButtonPressed()
    {
        if (stageManager != null)
        {
            stageManager.ActivateNextStage();
        }
    }
}
