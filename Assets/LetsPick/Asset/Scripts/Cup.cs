using UnityEngine;

public class Cup : MonoBehaviour
{
    // �J�b�v�̎�ނ��w�肷�邽�߂�Enum
    public enum CupType
    {
        Blue,   // �F�̃J�b�v
        Red,    // �ԐF�̃J�b�v
        Green,  // �ΐF�̃J�b�v
        Yellow  // ���F�̃J�b�v
    }

    // �J�b�v�̎�ނ��C���X�y�N�^�[�Őݒ�ł���悤�Ɍ��J
    public CombinationManager.CupType cupType; // CombinationManager��CupType���g�p
}
