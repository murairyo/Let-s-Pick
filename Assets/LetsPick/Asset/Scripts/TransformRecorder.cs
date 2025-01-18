using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
    // �f�t�H���g�̈ʒu�ƃX�P�[��
    private Vector3 defaultPosition;
    private Vector3 defaultScale;

    // �q���ڐ��p�̈ʒu�ƃX�P�[���i�ύX���K�v�ȏꍇ�̂ݎw��j
    [SerializeField] private Vector3 childPosition; // XYZ�P�ʂŕύX�\
    [SerializeField] private Vector3 childScale; // XYZ�P�ʂŕύX�\

    private bool isChildMode = false;

    void Start()
    {
        // �f�t�H���g�̈ʒu�ƃX�P�[�����L�^
        defaultPosition = transform.position;
        defaultScale = transform.localScale;
    }

    // �{�^���ŌĂяo�����؂�ւ��֐�
    public void ToggleTransform()
    {
        if (isChildMode)
        {
            // �f�t�H���g��Transform�ɖ߂�
            SetTransform(defaultPosition, defaultScale);
        }
        else
        {
            // �q���ڐ���Transform�ɕύX�i0�̏ꍇ�̓f�t�H���g�l��K�p�j
            Vector3 newPosition = CombineValues(defaultPosition, childPosition);
            Vector3 newScale = CombineValues(defaultScale, childScale);

            SetTransform(newPosition, newScale);
        }

        // ���[�h��؂�ւ�
        isChildMode = !isChildMode;
    }

    // Transform��ݒ肷��w���p�[�֐�
    private void SetTransform(Vector3 position, Vector3 scale)
    {
        transform.position = position;
        transform.localScale = scale;
    }

    // �f�t�H���g�l�Ǝq���ڐ��̒l������
    private Vector3 CombineValues(Vector3 defaultValues, Vector3 childValues)
    {
        return new Vector3(
            childValues.x != 0 ? childValues.x : defaultValues.x,
            childValues.y != 0 ? childValues.y : defaultValues.y,
            childValues.z != 0 ? childValues.z : defaultValues.z
        );
    }
}
