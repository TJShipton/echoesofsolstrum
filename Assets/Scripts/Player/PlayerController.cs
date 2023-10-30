using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour, IDamageable
{
    private float movementX;
    private float movementY;
    private Vector2 movementInput;  // Store the movement input


    [SerializeField]
    public Canvas EnemyCanvas;  // Drag your main UI Canvas here

    public Transform characterModel;
    public LayerMask groundLayer;

    public float speed = 50f;

    [SerializeField]
    private float jumpForce = 10f;  // Default value of 100, adjust in Inspector as needed
    public float doubleJumpVelocity = 15f;

    public float attackRange = 0.5f;
    public int maxHealth = 100;
    public int currentHealth;
    public Slider playerHealthBar;
    public TextMeshProUGUI playerHealthText;


    private bool shouldJump = false;
    private bool hasDoubleJumpUpgrade = false;
    private bool canDoubleJump = false;
    private bool shouldDoubleJump = false;
    private bool isGrounded;
    public float groundCheckDistance = 0.1f;
    private Rigidbody rb;
    private Animator animator;
    private WeaponManager weaponManager;

    private CurrencyManager currencyManager;




    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerController Start Called");

        rb = GetComponent<Rigidbody>();
        animator = transform.GetComponent<Animator>();
        weaponManager = GetComponent<WeaponManager>();
        currencyManager = CurrencyManager.instance;

        GameObject healthBarObj = GameObject.Find("PlayerHealthbar");
        if (healthBarObj != null)
        {
            playerHealthBar = healthBarObj.GetComponent<Slider>();
        }
        else
        {
            Debug.LogError("Health bar object not found.");
        }

        GameObject healthTextObj = GameObject.Find("PlayerHealthText");
        if (healthTextObj != null)
        {
            playerHealthText = healthTextObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Health text object not found.");
        }

        currentHealth = maxHealth;
        UpdateHealthUI();  // Ensure the UI is correct when the game starts

        // Apply permanent upgrades
        UpgradeManager.instance.ApplyPermanentUpgrades(this);
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void FixedUpdate()
    {

        // Update the horizontal movement
        Vector3 movement = new Vector3(movementInput.x, 0.0f);
        rb.AddForce(movement * speed, ForceMode.VelocityChange);

        // Jump
        isGrounded = IsGrounded();  // Update the isGrounded variable

        if (shouldJump)  // Check if jump input was received
        {
            Jump();
            shouldJump = false;  // Reset flag after jump is handled
            if (hasDoubleJumpUpgrade)
            {
                canDoubleJump = true;  // Enable double jump if the upgrade is active
                Debug.Log("Double jump enabled.");
            }
        }
        else if (shouldDoubleJump && canDoubleJump)  // Check if double jump input was received and double jump is allowed
        {
            DoubleJump();
            shouldDoubleJump = false;  // Reset flag after double jump is handled
            canDoubleJump = false;  // Disable further double jumps until grounded again
        }


    }
    private bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;  // Slightly raised origin to ensure the raycast starts above the ground
        Ray ray = new Ray(origin, Vector3.down);
        return Physics.Raycast(ray, groundCheckDistance, groundLayer);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();  // Read the input value from the context

        // Update the animator
        animator.SetFloat("IsRunning", Mathf.Abs(movementInput.x) > 0 ? 1f : 0f);

        // Determine the look direction
        if (Mathf.Abs(movementInput.x) > 0)
        {
            Vector3 lookDirection = movementInput.x > 0 ? Vector3.right : Vector3.left;
            transform.forward = lookDirection;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGrounded)  // Only jump if grounded and input just started
            {
                shouldJump = true;  // Set flag to true when jump input is received
            }
            else if (canDoubleJump)  // Allow double jump if not grounded but double jump is allowed
            {
                shouldDoubleJump = true;  // Set flag to true when double jump input is received
                Debug.Log("Double jump input received.");  // Log when double jump input is received
            }
        }
    }


    private void Jump()
    {
        Vector3 jumpVector = new Vector3(0f, jumpForce, 0f);
        rb.AddForce(jumpVector, ForceMode.VelocityChange);
    }

    private void DoubleJump()
    {
        Vector3 doubleJumpVector = new Vector3(0f, doubleJumpVelocity, 0f);
        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);  // Reset the vertical velocity before applying double jump force
        rb.AddForce(doubleJumpVector, ForceMode.VelocityChange);
    }


    public void OnAtck1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Atck1();
        }
    }

    public void OnAtck2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Atck2();
        }
    }

    private void Atck1()
    {
        // Select the slot 0
        InventoryManager.instance.SelectSlot(0);
        // Trigger the attack on the weapon in slot 0
        TriggerAttack();
    }

    private void Atck2()
    {
        // Check if there's a weapon in slot 1
        if (InventoryManager.instance.slots.Count > 1)
        {
            // Select the slot 1
            InventoryManager.instance.SelectSlot(1);
            // Trigger the attack on the weapon in slot 1
            TriggerAttack();
        }
        else
        {
            Debug.Log("No slot 1 available.");
        }
    }

    public void TriggerAttack()
    {
        Weapon currentWeapon = InventoryManager.instance.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            if (!currentWeapon.gameObject.activeSelf)
            {
                // If the current weapon is not active, activate it
                currentWeapon.gameObject.SetActive(true);

                // Optionally, deactivate the other weapon(s)
                foreach (Weapon weapon in InventoryManager.instance.GetAllWeapons())
                {
                    if (weapon != currentWeapon)
                    {
                        weapon.gameObject.SetActive(false);
                    }
                }
            }

            // Now trigger the attack
            currentWeapon.PrimaryAttack();
        }
        else
        {
            //Debug.LogWarning("No weapon is selected!");
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
        if (playerHealthBar == null || playerHealthText == null)
        {
            Debug.LogError("Health UI components are null!");
            return;
        }
        Debug.Log("UpdateHealthUI Called");
        playerHealthBar.maxValue = maxHealth;
        playerHealthBar.value = currentHealth;
        playerHealthText.text = currentHealth.ToString();
        Debug.Log("Player Health Bar: " + playerHealthBar);
        Debug.Log("Player Health Text: " + playerHealthText);

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
