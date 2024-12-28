using UnityEngine;

public class SideCollisionHandler : MonoBehaviour
{
    [Header("弾き飛ばす力の設定")]
    [SerializeField] private float forceMagnitude = 100f; // 弾き飛ばす力の大きさ

    private void OnTriggerEnter(Collider other)
    {
        // 衝突したオブジェクトがCharacterタグを持っているかチェック
        if (other.CompareTag("Character"))
        {
            Rigidbody characterRigidbody = other.GetComponent<Rigidbody>();

            if (characterRigidbody != null)
            {
                // 衝突の方向を計算
                Vector3 collisionDirection = (other.transform.position - transform.position).normalized;

                // Rigidbodyに力を加える
                characterRigidbody.AddForce(collisionDirection * forceMagnitude, ForceMode.Impulse);

                Debug.Log($"{other.gameObject.name} が {gameObject.name} のトリガーに入り、弾き飛ばされました。");
            }
            else
            {
                Debug.LogWarning("CharacterにRigidbodyがアタッチされていません。");
            }
        }
    }
}
