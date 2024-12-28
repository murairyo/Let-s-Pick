using UnityEngine;
using System.Collections;
using Oculus.Interaction;

public class TemporarilyDisableGrabbable : MonoBehaviour
{
    [Tooltip("掴みを制御するGrabbableコンポーネント")]
    [SerializeField] private Grabbable grabbable;

    [Tooltip("Grabbableを再アクティブ化するまでの遅延時間（秒）")]
    [SerializeField] private float reenableDelay = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Cup_SideColliderタグに触れた場合
        if (other.CompareTag("Cup_SideCollider"))
        {
            Debug.Log("Cup_SideColliderに接触。Grabbableを一時無効化します。");

            // Grabbableを一時的に無効化
            DisableGrabbable();
        }
    }

    private void DisableGrabbable()
    {
        if (grabbable != null)
        {
            // Grabbableを非アクティブ化
            grabbable.enabled = false;
            Debug.Log("Grabbableが無効化されました。");

            // 再アクティブ化を遅延実行
            StartCoroutine(ReenableGrabbableAfterDelay());
        }
        else
        {
            Debug.LogWarning("Grabbableが設定されていません。");
        }
    }

    private IEnumerator ReenableGrabbableAfterDelay()
    {
        // 指定した遅延時間を待機
        yield return new WaitForSeconds(reenableDelay);

        if (grabbable != null)
        {
            // Grabbableを再びアクティブ化
            grabbable.enabled = true;
            Debug.Log("Grabbableが再び有効化されました。");
        }
    }
}
