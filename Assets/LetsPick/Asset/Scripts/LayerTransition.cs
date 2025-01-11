using UnityEngine;

public class LayerTransition : MonoBehaviour
{
    private Animator _animator;
    private int _currentLayerIndex = 0;
    private int _totalLayers = 3; // ���C���[�̑���

    void Start()
    {
        _animator = GetComponent<Animator>();
        SetLayerWeight(_currentLayerIndex, 1); // ������ԂŃ��C���[1��L���ɂ���
    }

    // ���̃��C���[�Ɉڍs����
    public void TransitionToNextLayer()
    {
        // ���݂̃��C���[�𖳌��ɂ���
        SetLayerWeight(_currentLayerIndex, 0);

        // ���̃��C���[�̃C���f�b�N�X���v�Z
        _currentLayerIndex = (_currentLayerIndex + 1) % _totalLayers;

        // ���̃��C���[��L���ɂ���
        SetLayerWeight(_currentLayerIndex, 1);
    }

    // ����̃��C���[��L���ɂ���i���������p�j
    private void SetLayerWeight(int layerIndex, float weight)
    {
        _animator.SetLayerWeight(layerIndex, weight);
    }
}
