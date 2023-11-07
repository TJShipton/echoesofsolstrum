using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour, IDamageable
{
    private float movementX;
    private float movementY;
    private Vector2 movementInput;  // Store the movement input




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
    public float fallMultiplier = 2.5f; // This increases the fall speed
    public float lowJumpMultiplier = 2f; // This can be used to control the fall speed on a low jump

    private bool isDodging = false;
    public float dodgeDuration = 1.0f; // The duration of the dodge in seconds


    private Rigidbody rb;
    private Animator animator;
    private WeaponManager weaponManager;
    private CurrencyManager currencyManager;

    private bool isUIOpen = false;




    // Start is called before the first frame update
    void Start()
    {


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
        UpdateHealthbar();  // Ensure the healthbar is correct when the game starts

        // Apply permanent upgrades
        UpgradeManager.instance.ApplyPermanentUpgrades(this);
    }
    private void OnEnable()
    {
        // Subscribe to the OnMenuStatusChange event
        UIManager.OnMenuStatusChange += HandleUIStateChange;
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnMenuStatusChange event
        UIManager.OnMenuStatusChange -= HandleUIStateChange;
    }

    private void HandleUIStateChange(bool isMenuOpen)
    {
        isUIOpen = isMenuOpen; // Update the internal state.

        if (isUIOpen)
        {
            StopAllActions();
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Before any player input, check if the UI is open
        if (UIManager.Instance.IsAnyMenuOpen)
        {
            // If a menu is open, don't process any gameplay input
            return;
        }


    }

    private void StopAllActions()
    {
        // Stop all movement by setting velocity to zero
        rb.velocity = Vector3.zero;

        movementInput = Vector2.zero;  // This ensures no new force will be applied as long as the UI is open

        // Also stop any animations or actions that should cease when UI is open
        animator.SetFloat("IsRunning", 0f);
        animator.SetBool("IsGrounded", isGrounded);
        animator.ResetTrigger("JumpTrigger");
        animator.ResetTrigger("DoubleJumpTrigger");
        animator.SetBool("IsFalling", false);


    }

    private void FixedUpdate()
    {
        if (isUIOpen) return;  // Don't apply physics updates if the UI is open

        // Update the horizontal movement
        Vector3 movement = new Vector3(movementInput.x, 0.0f);
        rb.AddForce(movement * speed, ForceMode.VelocityChange);

        // Jump
        isGrounded = IsGrounded();
        animator.SetBool("IsGrounded", isGrounded);  // Update the isGrounded animator

        if (shouldJump)  // Check if jump input was received
        {
            Jump();
            animator.SetTrigger("JumpTrigger");
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

        // Check for falling
        if (rb.velocity.y < -0.1f)
        {
            // The player is falling
            animator.SetBool("IsFalling", true);

            // Apply the fall multiplier
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // Replace "Jump" with your actual jump input name
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        else
        {
            // If not falling
            animator.SetBool("IsFalling", false);
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
        // Only process input if no UI menu is open
        if (isUIOpen) return;

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
        // Only process input if no UI menu is open

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
    { // Only process input if no UI menu is open


        Vector3 jumpVector = new Vector3(0f, jumpForce, 0f);
        rb.AddForce(jumpVector, ForceMode.VelocityChange);

    }

    private void DoubleJump()
    {
        // Only process input if no UI menu is open

        animator.SetTrigger("DoubleJumpTrigger");
        Vector3 doubleJumpVector = new Vector3(0f, doubleJumpVelocity, 0f);
        rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);  // Reset the vertical velocity before applying double jump force
        rb.AddForce(doubleJumpVector, ForceMode.VelocityChange);

    }


    public void OnDodge(InputAction.CallbackContext context)
    {

        StartCoroutine(Dodge());

    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        animator.SetTrigger("DodgeTrigger"); // Trigger dodge animation

        // Wait for the duration of the dodge
        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false; // Player is no longer dodging
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

        // Activate the weapon during the attack
        if (weaponManager.currentWeapon != null)
        {
            weaponManager.currentWeapon.gameObject.SetActive(true);
        }

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

            // Initialize the weapon's animator before triggering the attack
            Animator animator = GetComponent<Animator>(); // Assuming the animator is on the same GameObject as this script
            currentWeapon.InitWeaponAnimator(animator);

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
        if (isDodging)
        {
            // Player is dodging, don't take damage
            return;
        }

        currentHealth -= damageAmount;


        if (currentHealth > 0)
        {
            UpdateHealthbar();
        }
        if (currentHealth <= 0)  // Check if current health is 0 or below
        {
            Die();
            Destroy(gameObject);
        }
    }

    public void UpdateHealthbar()
    {
        if (playerHealthBar == null || playerHealthText == null)
        {
            Debug.LogError("Health UI components are null!");
            return;
        }

        playerHealthBar.maxValue = maxHealth;
        playerHealthBar.value = currentHealth;
        playerHealthText.text = currentHealth.ToString();


    }

    private void Die()
    {
        Debug.Log("Player has died");
        // STILL TO DO --- trigger a death animation, end the game.
    }


    //Sol pick up function
    private void OnTriggerEnter(Collider other)
    {
        SolObject solObject = other.gameObject.GetComponent<SolObject>();
        if (solObject != null)
        {
            solObject.PickUpSol();
        }
    }


    //Apply upgrade 
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
