using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stages; // �S�X�e�[�W��o�^
    [SerializeField] private GameObject stageClearUI; // �l�N�X�g�{�^��UI
    [SerializeField] private GameObject exitButtonUI; // Exit�{�^��UI
    [SerializeField] private GameObject finishUI; // Finish UI
    [SerializeField] private AudioSource clearSound; // �X�e�[�W�N���A���̉�
    [SerializeField] private ParticleSystem clearEffect; // �X�e�[�W�N���A���̃G�t�F�N�g

    private int currentStageIndex = 0; // ���݂̃X�e�[�W�C���f�b�N�X
    private int totalCups; // �X�e�[�W���̃J�b�v�̑���
    private int filledCups; // ���܂����J�b�v�̐�

    /// <summary>
    /// �ŏ��̃X�e�[�W���������i�O������Ăяo�����j
    /// </summary>
    public void InitializeFirstStage()
    {
        Debug.Log("�ŏ��̃X�e�[�W��������");
        InitializeCurrentStage();
    }

    /// <summary>
    /// ���݂̃X�e�[�W��������
    /// </summary>
    private void InitializeCurrentStage()
    {
        if (currentStageIndex >= stages.Length)
        {
            Debug.LogWarning("�S�X�e�[�W���N���A����܂����B");
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

        Debug.Log($"�X�e�[�W{currentStageIndex + 1}��������: �J�b�v�� {totalCups}");
    }

    /// <summary>
    /// ���݂̃X�e�[�W���I�����A���̃X�e�[�W������
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
            Debug.Log("�Ō�̃X�e�[�W���N���A���܂����I");
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
    /// �J�b�v�����܂邽�тɌĂяo�����
    /// </summary>
    private void HandleCupFilled()
    {
        filledCups++;
        Debug.Log($"�J�b�v�����܂�܂���: {filledCups}/{totalCups}");

        if (filledCups >= totalCups)
        {
            TriggerStageClear();
        }
    }

    /// <summary>
    /// �X�e�[�W�N���A���̏���
    /// </summary>
    private void TriggerStageClear()
    {
        Debug.Log($"�X�e�[�W{currentStageIndex + 1}�N���A�I");

        // Finish UI �̕\��
        if (finishUI != null)
        {
            finishUI.SetActive(true);
            Debug.Log("Finish UI is now visible.");
        }

        // �I���̍��}�̉����Đ�
        if (clearSound != null)
        {
            clearSound.Play();
            Debug.Log("Clear sound played.");
        }

        // �G�t�F�N�g�𔭓�
        if (clearEffect != null)
        {
            clearEffect.Play();
            Debug.Log("Clear effect played.");
        }

        // �ŏI�X�e�[�W�̏ꍇ�AExit�{�^����\�����ďI��
        if (currentStageIndex == stages.Length - 1)
        {
            Debug.Log("�ŏI�X�e�[�W���N���A���܂����I");
            ShowExitButton();
            return;
        }

        // �ʏ�̃X�e�[�W�N���A����
        if (stageClearUI != null && currentStageIndex < stages.Length - 1)
        {
            stageClearUI.transform.localScale = Vector3.one;
            stageClearUI.SetActive(true);
            Debug.Log("StageClearUI is now visible with scale reset to 1.");
        }
    }

    /// <summary>
    /// Exit�{�^����\������
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
