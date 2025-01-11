using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stages; // 全ステージを登録
    [SerializeField] private GameObject stageClearUI; // ネクストボタンUI
    [SerializeField] private GameObject exitButtonUI; // ExitボタンUI
    [SerializeField] private GameObject finishUI; // Finish UI
    [SerializeField] private AudioSource clearSound; // ステージクリア時の音
    [SerializeField] private ParticleSystem clearEffect; // ステージクリア時のエフェクト

    private int currentStageIndex = 0; // 現在のステージインデックス
    private int totalCups; // ステージ内のカップの総数
    private int filledCups; // 埋まったカップの数

    /// <summary>
    /// 最初のステージを初期化（外部から呼び出される）
    /// </summary>
    public void InitializeFirstStage()
    {
        Debug.Log("最初のステージを初期化");
        InitializeCurrentStage();
    }

    /// <summary>
    /// 現在のステージを初期化
    /// </summary>
    private void InitializeCurrentStage()
    {
        if (currentStageIndex >= stages.Length)
        {
            Debug.LogWarning("全ステージがクリアされました。");
            return;
        }

        GameObject currentStage = stages[currentStageIndex];
        filledCups = 0;

        GameObject[] cups = GameObject.FindGameObjectsWithTag("Bot_Collider");
        totalCups = cups.Length;

        foreach (var cup in cups)
        {
            var colliderHandler = cup.GetComponent<ColliderHandler>();
            if (colliderHandler != null)
            {
                colliderHandler.OnCupFilled += HandleCupFilled;
                Debug.Log($"{cup.name}: ColliderHandler event registered.");
            }

            var colliderHandlerStage3 = cup.GetComponent<ColliderHandlerStage3>();
            if (colliderHandlerStage3 != null)
            {
                colliderHandlerStage3.OnCupFilled += HandleCupFilled;
                Debug.Log($"{cup.name}: ColliderHandlerStage3 event registered.");
            }
        }

        Debug.Log($"ステージ{currentStageIndex + 1}を初期化: カップ数 {totalCups}");
    }

    /// <summary>
    /// 現在のステージを終了し、次のステージを準備
    /// </summary>
    public void ActivateNextStage()
    {
        GameObject currentStage = stages[currentStageIndex];
        currentStage.SetActive(false);

        ColliderHandler[] cups = currentStage.GetComponentsInChildren<ColliderHandler>();
        foreach (var cup in cups)
        {
            cup.OnCupFilled -= HandleCupFilled;
            Debug.Log($"{cup.name}: ColliderHandler event unregistered.");
        }

        if (currentStageIndex == stages.Length - 1)
        {
            Debug.Log("最後のステージをクリアしました！");
            ShowExitButton();
            return;
        }

        currentStageIndex++;
        Debug.Log($"Current Stage Index: {currentStageIndex}, Total Stages: {stages.Length}");

        if (currentStageIndex < stages.Length)
        {
            GameObject nextStage = stages[currentStageIndex];
            nextStage.SetActive(true);
            InitializeCurrentStage();
        }
    }

    /// <summary>
    /// カップが埋まるたびに呼び出される
    /// </summary>
    private void HandleCupFilled()
    {
        filledCups++;
        Debug.Log($"カップが埋まりました: {filledCups}/{totalCups}");

        if (filledCups >= totalCups)
        {
            TriggerStageClear();
        }
    }

    /// <summary>
    /// ステージクリア時の処理
    /// </summary>
    private void TriggerStageClear()
    {
        Debug.Log($"ステージ{currentStageIndex + 1}クリア！");

        // Finish UI の表示
        if (finishUI != null)
        {
            finishUI.SetActive(true);
            Debug.Log("Finish UI is now visible.");
        }

        // 終了の合図の音を再生
        if (clearSound != null)
        {
            clearSound.Play();
            Debug.Log("Clear sound played.");
        }

        // エフェクトを発動
        if (clearEffect != null)
        {
            clearEffect.Play();
            Debug.Log("Clear effect played.");
        }

        // 最終ステージの場合、Exitボタンを表示して終了
        if (currentStageIndex == stages.Length - 1)
        {
            Debug.Log("最終ステージをクリアしました！");
            ShowExitButton();
            return;
        }

        // 通常のステージクリア処理
        if (stageClearUI != null && currentStageIndex < stages.Length - 1)
        {
            stageClearUI.transform.localScale = Vector3.one;
            stageClearUI.SetActive(true);
            Debug.Log("StageClearUI is now visible with scale reset to 1.");
        }
    }

    /// <summary>
    /// Exitボタンを表示する
    /// </summary>
    private void ShowExitButton()
    {
        if (exitButtonUI != null)
        {
            exitButtonUI.transform.localScale = Vector3.one;
            exitButtonUI.SetActive(true);
            Debug.Log("ExitButtonUI is now visible with scale reset to 1.");
        }
    }
}
