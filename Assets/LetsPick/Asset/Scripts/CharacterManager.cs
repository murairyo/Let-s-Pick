using UnityEngine;

public class RespawnObject : MonoBehaviour
{
    public float thresholdY = 0f; // 物体が落ちる高さの閾値（床面）
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private int grabCount = 0; // 掴まれた回数を管理する変数

    void Start()
    {
        // 初期位置と初期回転を保存
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // 物体のY座標が閾値を下回った場合にリスポーン処理を実行
        if (transform.position.y < thresholdY)
        {
            Respawn();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトのタグが "Floor" の場合にリスポーン処理を実行
        if (collision.gameObject.CompareTag("Floor"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // 初期位置に物体を移動し、速度をリセット
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // リスポーン時に掴まれた回数をリセット
        grabCount = 0;
    }

    // 掴まれた際に呼び出される関数（例）
    public void OnGrab()
    {
        grabCount++;
        Debug.Log("Object grabbed. Current grab count: " + grabCount);
    }
}
