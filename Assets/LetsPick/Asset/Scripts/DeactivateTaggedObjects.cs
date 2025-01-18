using UnityEngine;

public class DeactivateTaggedObjects : MonoBehaviour
{
    // ボタンで呼び出される関数
    public void DeactivateActiveObjectsWithTag()
    {
        // "Cup_SideCollider" タグのついたすべてのオブジェクトを取得
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Cup_SideCollider");

        foreach (GameObject obj in objectsWithTag)
        {
            // アクティブ状態か確認
            if (obj.activeSelf)
            {
                // 非アクティブ化
                obj.SetActive(false);
            }
        }
    }
}
