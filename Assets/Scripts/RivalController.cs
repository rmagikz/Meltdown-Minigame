using System.Collections;
using UnityEngine;
using System.Linq;

public class RivalController : MonoBehaviour
{
    public enum MoveType {None, High, Low};

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform stickHigh, stickLow;
    [SerializeField] BoxCollider triggerCollider;
    [SerializeField] BoxCollider boxCollider;

    [SerializeField] float successRate;

    [SerializeField] float jumpForce;

    [SerializeField] float colliderHeightCrouch;
    [SerializeField] float colliderCenterCrouch;

    private float colliderHeightStanding;
    private float colliderCenterStanding;

    private float jumpCooldown = 0.5f;
    private float jumpTimer = 0.5f;

    private static bool canAct;

    public static void ToggleRivals()
    {
        canAct = !canAct;
    }

    void Start()
    {
        colliderHeightStanding = boxCollider.size.y;
        colliderCenterStanding = boxCollider.center.y;
    }

    void Update()
    {
        jumpTimer = jumpTimer >= 0 ? jumpTimer++ : jumpTimer;
    }

    void SetColliderSizeCenter(float centerY, float heightY)
    {
        Vector3 colliderSize = boxCollider.size;
        colliderSize.y = heightY;
        boxCollider.size = colliderSize;
        Vector3 colliderCenter = boxCollider.center;
        colliderCenter.y = centerY;
        boxCollider.center = colliderCenter;
    }

    void Jump(Vector3 position)
    {
        animator.SetTrigger("Jumping");
        rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!canAct) return;

        if (Random.value * 100f > successRate) return;

        if (collider.gameObject.transform == stickHigh)
        {
            SetColliderSizeCenter(colliderCenterCrouch, colliderHeightCrouch);
            animator.SetBool("Crouching", true);
        }
        if (collider.gameObject.transform == stickLow)
        {
            if (Random.value < 0.5f)
            {
                Jump(Vector3.zero);
            }
            else
            {
                if (jumpTimer == jumpCooldown)
                    StartCoroutine(JumpToNextPedestal());
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        SetColliderSizeCenter(colliderCenterStanding, colliderHeightStanding);
        animator.SetBool("Crouching", false);
    }

    IEnumerator JumpToNextPedestal()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        triggerCollider.enabled = false;
        boxCollider.enabled = false;
        
        animator.SetTrigger("Jumping");

        Transform[] pedestals = FindTwoClosest();
        Transform chosenPedestal = Random.value < 0.5f ? pedestals[0] : pedestals[1];

        Vector3 first = transform.position;
        Vector3 last = chosenPedestal.position + new Vector3(0,2f,0);
        Vector3 middle = first + (last - first) / 2f;
        middle.y += Vector3.Distance(first, last) / 2f;

        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            transform.position = PointOnBezier(first, middle, last, i);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        rb.useGravity = true;
        rb.isKinematic = false;
        boxCollider.enabled = true;
        triggerCollider.enabled = true;

        jumpCooldown = 0;

        yield return null;
    }

    Transform[] FindTwoClosest()
    {
        Transform[] sorted = GameManager.instance.pedestals.OrderBy(obj => Vector3.Distance(transform.position, obj.position)).ToArray();;

        return new Transform[2] {sorted[1], sorted[2]};
    }

    Vector3 PointOnBezier(Vector3 first, Vector3 second, Vector3 last, float t)
    {
        Vector3 firstSecond = Vector3.Lerp(first, second, t);
        Vector3 secondLast = Vector3.Lerp(second, last, t);

        return Vector3.Lerp(firstSecond, secondLast, t);
    }
}
