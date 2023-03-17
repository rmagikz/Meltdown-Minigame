using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airSpeed;

    private float mouseX, vertical;
    private float playerRotation;
    private bool jumping;
    private bool crouching;

    private bool paused;

    private float sensitivity = 1f;

    void Start()
    {
        paused = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            GameManager.instance.Pause();
        }

        if (paused) return;

        mouseX= Input.GetAxisRaw("Mouse X");
        vertical = Input.GetAxisRaw("Vertical");

        gameObject.transform.Rotate(new Vector3(0,mouseX * sensitivity,0));

        if (vertical == 0 || crouching)
            animator.SetBool("Running", false);
        else 
            animator.SetBool("Running", true);

        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumping = true;
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

    public void ToggleController()
    {
        paused = !paused;
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;
    }

    private void ProcessMove()
    {
        if (crouching) return;

        Vector3 forwardDirection = (transform.position - Camera.main.transform.position).normalized; 
        Vector3 velocity = forwardDirection * vertical * (moveSpeed) * Time.deltaTime;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (jumping)
        {
            rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
            jumping = false;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }
}
