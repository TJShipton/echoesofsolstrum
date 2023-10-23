using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public int maxHealth = 100;
    public int currentHealth;
    public Slider playerHealthBar;
    public TextMeshProUGUI playerHealthText;

    private bool hasDoubleJumpUpgrade = false;
    private bool canDoubleJump = false;
    private bool isGrounded;
    private Rigidbody rb;
    private Animator animator;
    private WeaponManager weaponManager;

    private CurrencyManager currencyManager;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetComponent<Animator>();
        weaponManager = GetComponent<WeaponManager>();
        currencyManager = CurrencyManager.instance;

        currentHealth = maxHealth;
        //playerHealthText.text = currentHealth.ToString();
        UpdateHealthUI();  // Ensure the UI is correct when the game starts

        // Apply permanent upgrades
        UpgradeManager.instance.ApplyPermanentUpgrades(this);
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

                // Only enable double jump if the upgrade has been purchased
                if (hasDoubleJumpUpgrade)
                {
                    canDoubleJump = true;
                }
            }
            else if (canDoubleJump && hasDoubleJumpUpgrade)  // Check if hasDoubleJumpUpgrade is true
            {
                rb.velocity = new Vector3(rb.velocity.x, doubleJumpVelocity, 0f);
                canDoubleJump = false;
                animator.SetTrigger("DoubleJumpTrigger");
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

    public void TakeDamage(int damageAmount, Canvas HUDCanvas)
    {
        currentHealth -= damageAmount;

        Debug.Log("Player health: " + currentHealth);  // Log to check the current health

        if (currentHealth > 0)
        {
            UpdateHealthUI();
        }
        if (currentHealth <= 0)  // Check if current health is 0 or below
        {
            Die();
            Destroy(gameObject);
        }
    }

    public void UpdateHealthUI()
    {
        playerHealthBar.maxValue = maxHealth;
        playerHealthBar.value = currentHealth;
        playerHealthText.text = currentHealth.ToString();
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



    public void ApplyUpgrade(IPlayerUpgrade upgrade)
    {
        upgrade.ApplyUpgrade(this);
    }

    public void EnableDoubleJump()
    {
        // Set hasDoubleJumpUpgrade to true when the upgrade is applied
        hasDoubleJumpUpgrade = true;
    }
}
