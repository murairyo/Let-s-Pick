using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public float thresholdY = 0f; // 物体が落ちる高さの閾値（床面）

    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position; // プレファブの初期位置をリスポーンポイントとして設定
        initialRotation = transform.rotation; // プレファブの初期回転をリスポーン時に使用
    }

    void Update()
    {
        // 物体のY座標が閾値よりも低い場合、リスポーンさせる
        if (transform.position.y < thresholdY)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // リスポーン位置に物体を移動し、速度をリセットする
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
