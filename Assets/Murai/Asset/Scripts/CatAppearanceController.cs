using UnityEngine;
using System.Collections;

public class CatAppearanceController : MonoBehaviour
{
    [Header("Textures for Each State")]
    [SerializeField] private Texture waitingTexture;    // ????????
    [SerializeField] private Texture grabbedTexture;    // ?????????
    [SerializeField] private Texture dropTexture;       // ??????????

    private Material catMaterial;
    private Animator animator;

    // ????????
    private bool isGrabbed = false;
    private bool onCollision = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // ??????SkinnedMeshRenderer????????????????
        SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer>();
        if (smr != null)
        {
            catMaterial = smr.material;
        }
        else
        {
            Debug.LogWarning("SkinnedMeshRenderer????????????????");
        }

        // ???????????????
        UpdateTexture();
    }

    public void OnSelect()
    {
        isGrabbed = true;
        onCollision = false;
        UpdateTexture();

        animator.SetBool("IsGrabbed", true);
        animator.SetBool("OnCollision", false);
    }

    public void OnUnselect()
    {
        isGrabbed = false;
        onCollision = false;
        UpdateTexture();

        animator.SetBool("IsGrabbed", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ???????????????
        if (!isGrabbed)
        {
            onCollision = true;
            UpdateTexture();

            animator.SetBool("OnCollision", true);
            StartCoroutine(ResetOnCollisionState());
        }
    }

    private IEnumerator ResetOnCollisionState()
    {
        float bounceAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(bounceAnimationLength);

        onCollision = false;
        UpdateTexture();
        animator.SetBool("OnCollision", false);
    }

    private void UpdateTexture()
    {
        if (catMaterial == null) return;

        if (isGrabbed)
        {
            catMaterial.mainTexture = grabbedTexture;
        }
        else if (onCollision)
        {
            catMaterial.mainTexture = dropTexture;
        }
        else
        {
            catMaterial.mainTexture = waitingTexture;
        }
    }
}
