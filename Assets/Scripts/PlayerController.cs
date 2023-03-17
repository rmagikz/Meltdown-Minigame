using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airSpeed;

    private float horizontal, vertical;
    private float playerRotation;
    private float airSpeedMod;
    private bool jumping;
    private bool crouching;

    private bool paused;

    private float sensitivity = 1f;

    void Start()
    {
        paused = true;

        airSpeedMod = 1;
    }

    public void ToggleController()
    {
        paused = !paused;
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            GameManager.instance.Pause();
        }

        if (paused) return;

        horizontal= Input.GetAxisRaw("Mouse X");
        vertical = Input.GetAxisRaw("Vertical");

        gameObject.transform.Rotate(new Vector3(0,horizontal * sensitivity,0));

        if (vertical == 0 || crouching)
            animator.SetBool("Running", false);
        else 
            animator.SetBool("Running", true);

        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumping = true;
                airSpeedMod = airSpeed;
                animator.SetBool("Running", false);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                crouching = !crouching;

                boxCollider.center = boxCollider.center.y > 0.30f ? new Vector3(0,0.30f,0) : new Vector3(0, 0.92f, 0);
                boxCollider.size = boxCollider.size.y > 0.616f ? new Vector3(1, 0.616f, 1) : new Vector3(1, 1.85f, 1);

                animator.SetBool("Crouching", animator.GetBool("Crouching") == true ? false : true);
            }
        }
    }

    void FixedUpdate()
    {
        ProcessMove();
    }

    private void ProcessMove()
    {
        if (crouching) return;

        Vector3 direction = (transform.position - Camera.main.transform.position).normalized;
        
        Vector3 velocity = direction * vertical * (moveSpeed * airSpeedMod) * Time.deltaTime;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (jumping)
        {
            rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
            airSpeedMod = 1;
            jumping = false;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }
}
