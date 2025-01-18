using System.Collections.Generic;
using UnityEngine;

public class TransformManager : MonoBehaviour
{
    // 管理するTransformRecorderオブジェクトのリスト
    [SerializeField] private List<TransformRecorder> recorders = new List<TransformRecorder>();

    // トリガーを一括で発火
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

    // 動的にオブジェクトを追加
    public void AddRecorder(TransformRecorder recorder)
    {
        if (!recorders.Contains(recorder))
        {
            recorders.Add(recorder);
        }
    }

    // 動的にオブジェクトを削除
    public void RemoveRecorder(TransformRecorder recorder)
    {
        if (recorders.Contains(recorder))
        {
            recorders.Remove(recorder);
        }
    }
}
