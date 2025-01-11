using UnityEngine;

public class DeactivateRoot : MonoBehaviour
{
    // 大元の親オブジェクト（ルートオブジェクト）を非アクティブ化する関数
    public void DeactivateRootObject()
    {
        // ルートオブジェクトを非アクティブ化
        transform.root.gameObject.SetActive(false);
    }
}
