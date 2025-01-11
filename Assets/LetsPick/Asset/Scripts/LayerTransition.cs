using UnityEngine;

public class LayerTransition : MonoBehaviour
{
    private Animator _animator;
    private int _currentLayerIndex = 0;
    private int _totalLayers = 3; // レイヤーの総数

    void Start()
    {
        _animator = GetComponent<Animator>();
        SetLayerWeight(_currentLayerIndex, 1); // 初期状態でレイヤー1を有効にする
    }

    // 次のレイヤーに移行する
    public void TransitionToNextLayer()
    {
        // 現在のレイヤーを無効にする
        SetLayerWeight(_currentLayerIndex, 0);

        // 次のレイヤーのインデックスを計算
        _currentLayerIndex = (_currentLayerIndex + 1) % _totalLayers;

        // 次のレイヤーを有効にする
        SetLayerWeight(_currentLayerIndex, 1);
    }

    // 特定のレイヤーを有効にする（内部処理用）
    private void SetLayerWeight(int layerIndex, float weight)
    {
        _animator.SetLayerWeight(layerIndex, weight);
    }
}
