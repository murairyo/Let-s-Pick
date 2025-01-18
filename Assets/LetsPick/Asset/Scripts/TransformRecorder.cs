using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
    // デフォルトの位置とスケール
    private Vector3 defaultPosition;
    private Vector3 defaultScale;

    // 子供目線用の位置とスケール（変更が必要な場合のみ指定）
    [SerializeField] private Vector3 childPosition; // XYZ単位で変更可能
    [SerializeField] private Vector3 childScale; // XYZ単位で変更可能

    private bool isChildMode = false;

    void Start()
    {
        // デフォルトの位置とスケールを記録
        defaultPosition = transform.position;
        defaultScale = transform.localScale;
    }

    // ボタンで呼び出される切り替え関数
    public void ToggleTransform()
    {
        if (isChildMode)
        {
            // デフォルトのTransformに戻す
            SetTransform(defaultPosition, defaultScale);
        }
        else
        {
            // 子供目線のTransformに変更（0の場合はデフォルト値を適用）
            Vector3 newPosition = CombineValues(defaultPosition, childPosition);
            Vector3 newScale = CombineValues(defaultScale, childScale);

            SetTransform(newPosition, newScale);
        }

        // モードを切り替え
        isChildMode = !isChildMode;
    }

    // Transformを設定するヘルパー関数
    private void SetTransform(Vector3 position, Vector3 scale)
    {
        transform.position = position;
        transform.localScale = scale;
    }

    // デフォルト値と子供目線の値を合成
    private Vector3 CombineValues(Vector3 defaultValues, Vector3 childValues)
    {
        return new Vector3(
            childValues.x != 0 ? childValues.x : defaultValues.x,
            childValues.y != 0 ? childValues.y : defaultValues.y,
            childValues.z != 0 ? childValues.z : defaultValues.z
        );
    }
}
