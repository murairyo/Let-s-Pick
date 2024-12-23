using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NonPenetration : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Rigidbodyの参照を取得
        rb = GetComponent<Rigidbody>();

        // Rigidbodyの設定を変更
        rb.isKinematic = false;  // Rigidbodyを物理エンジンで制御
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // 衝突検出モードを連続に設定
        rb.interpolation = RigidbodyInterpolation.Interpolate; // 補間を有効にすることで、スムーズな動きを実現
    }

    // オブジェクトが別のColliderに衝突した時の処理
    void OnCollisionEnter(Collision collision)
    {
        // ここに衝突時の処理を追加できます（例：音を鳴らす、エフェクトを表示するなど）
        Debug.Log("Collision detected with " + collision.gameObject.name);
    }
}
