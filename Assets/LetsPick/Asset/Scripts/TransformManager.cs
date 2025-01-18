using System.Collections.Generic;
using UnityEngine;

public class TransformManager : MonoBehaviour
{
    // �Ǘ�����TransformRecorder�I�u�W�F�N�g�̃��X�g
    [SerializeField] private List<TransformRecorder> recorders = new List<TransformRecorder>();

    // �g���K�[���ꊇ�Ŕ���
    public void ToggleAllTransforms()
    {
        foreach (TransformRecorder recorder in recorders)
        {
            if (recorder != null)
            {
                recorder.ToggleTransform();
            }
        }
    }

    // ���I�ɃI�u�W�F�N�g��ǉ�
    public void AddRecorder(TransformRecorder recorder)
    {
        if (!recorders.Contains(recorder))
        {
            recorders.Add(recorder);
        }
    }

    // ���I�ɃI�u�W�F�N�g���폜
    public void RemoveRecorder(TransformRecorder recorder)
    {
        if (recorders.Contains(recorder))
        {
            recorders.Remove(recorder);
        }
    }
}
