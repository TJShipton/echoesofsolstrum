using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField]
    public Canvas EnemyCanvas;  // Drag your main UI Canvas here

    public Transform characterModel;
    public LayerMask groundLayer;

    public float speed = 5f;
    public float acceleration = 500f;  // New variable for acceleration, adjust in Inspector as needed
    [SerializeField]
    private float jumpVelocity = 10f;  // Default value of 100, adjust in Inspector as needed
    public float doubleJumpVelocity = 15f;
    public float extraGravityForce = 0f;
    public float attackRange = 0.5f;

    public int health = 100;

    private bool canDoubleJump = true;

    private Rigidbody rb;
    private Animator animator;
    private WeaponManager weaponManager;

    // Flag to check if the player is on the ground
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetComponent<Animator>();
        weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Using Transform.Translate to move the player
        transform.Translate(Vector3.right * moveHorizontal * speed * Time.deltaTime, Space.World);

        bool isMoving = moveHorizontal != 0;
        animator.SetFloat("IsRunning", isMoving ? 1f : 0f);

        if (isMoving)
        {
            Vector3 lookDirection = moveHorizontal > 0 ? Vector3.right : Vector3.left;
            transform.forward = lookDirection;
        }

        float raycastDistance = 0.2f;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.2f;
        isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);

        if (isGrounded)
        {
            canDoubleJump = true;
            animator.ResetTrigger("DoubleJumpTrigger");
        }

        animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0f);
                animator.SetTrigger("JumpTrigger");
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector3(rb.velocity.x, doubleJumpVelocity, 0f);
                canDoubleJump = false;
                animator.SetTrigger("DoubleJumpTrigger");
                Debug.Log("Double Jump Triggered. Animator State: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Double Jump") + ", Rigidbody Velocity: " + rb.velocity);
            }
        }

        // Damping the horizontal velocity to respect the speed limit
        float clampedVelocityX = Mathf.Clamp(rb.velocity.x, -speed, speed);
        rb.velocity = new Vector3(clampedVelocityX, rb.velocity.y, 0f);

        // Apply extra gravity force when the player is falling This helps with timing roll animation.
        if (!isGrounded && rb.velocity.y < 0)
        {
            rb.velocity += Vector3.down * extraGravityForce * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            MainAttack();
        }
    }

    private void MainAttack()
    {
        if (weaponManager.currentWeapon != null)
        {
            weaponManager.currentWeapon.PrimaryAttack();
        }
    }

    public void TakeDamage(int damageAmount, Canvas EnemyCanvas)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died");
        // STILL TO DO --- trigger a death animation, end the game.
    }

    private void OnTriggerEnter(Collider other)
    {
        SolObject solObject = other.gameObject.GetComponent<SolObject>();
        if (solObject != null)
        {
            solObject.PickUpSol();
        }
    }
}
