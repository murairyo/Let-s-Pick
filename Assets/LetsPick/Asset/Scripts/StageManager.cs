using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] stages; // �S�X�e�[�W��o�^
    [SerializeField] private GameObject stageClearUI; // �l�N�X�g�{�^��UI
    [SerializeField] private GameObject exitButtonUI; // Exit�{�^��UI
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

        // ���݂̃X�e�[�W���擾
        GameObject currentStage = stages[currentStageIndex];
        filledCups = 0;

        // �^�O�ŃJ�b�v���擾
        GameObject[] cups = GameObject.FindGameObjectsWithTag("Bot_Collider");
        totalCups = cups.Length;

        foreach (var cup in cups)
        {
            // �e�J�b�v�̋������m�F���A�C�x���g��o�^
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
        // ���݂̃X�e�[�W���A�N�e�B�u��
        GameObject currentStage = stages[currentStageIndex];
        currentStage.SetActive(false);

        // �J�b�v�̃��X�i�[����
        ColliderHandler[] cups = currentStage.GetComponentsInChildren<ColliderHandler>();
        foreach (var cup in cups)
        {
            cup.OnCupFilled -= HandleCupFilled;
            Debug.Log($"{cup.name}: ColliderHandler event unregistered.");
        }

        // �Ō�̃X�e�[�W�̏ꍇ�AExit�{�^����\��
        if (currentStageIndex == stages.Length - 1)
        {
            Debug.Log("�Ō�̃X�e�[�W���N���A���܂����I");
            ShowExitButton();
            return; // ���̃X�e�[�W�ɐi�܂Ȃ�
        }

        // ���̃X�e�[�W�ɐi��
        currentStageIndex++;
        Debug.Log($"Current Stage Index: {currentStageIndex}, Total Stages: {stages.Length}");

        if (currentStageIndex < stages.Length)
        {
            GameObject nextStage = stages[currentStageIndex];
            nextStage.SetActive(true); // ���̃X�e�[�W���A�N�e�B�u��
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

        // ���ׂẴJ�b�v�����܂����ꍇ�A�X�e�[�W�N���A
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

        // �ŏI�X�e�[�W�̏ꍇ�AExit�{�^����\��
        if (currentStageIndex == stages.Length - 1)
        {
            Debug.Log("�ŏI�X�e�[�W���N���A���܂����I");
            ShowExitButton();
            return; // ���̃X�e�[�W�ɐi�܂Ȃ�
        }

        // �ʏ�̃X�e�[�W�N���A����
        if (stageClearUI != null && currentStageIndex < stages.Length - 1)
        {
            stageClearUI.transform.localScale = Vector3.one; // �X�P�[�������Z�b�g
            stageClearUI.SetActive(true); // �l�N�X�g�{�^����\��
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
            exitButtonUI.transform.localScale = Vector3.one; // �X�P�[�������Z�b�g
            exitButtonUI.SetActive(true); // Exit�{�^����\��
            Debug.Log("ExitButtonUI is now visible with scale reset to 1.");
        }
    }
}
