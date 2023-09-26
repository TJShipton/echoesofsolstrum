using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Canvas uiCanvas;  // Drag your main UI Canvas here

    public Transform characterModel;

    public float speed = 5f;
    public float jumpForce = 5f;
    public float attackRange = 0.5f;
    public int health = 100;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement = Vector3.zero;

    private WeaponManager weaponManager;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {


        // Player Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

        // Check for a left mouse button click and attack 
        if (Input.GetMouseButtonDown(0))
        {
            MainAttack();
        }

    }


    private void MainAttack()
    {
        // Assume weaponManager is a reference to your WeaponManager script attached to the player
        if (weaponManager.currentWeapon != null)
        {
            weaponManager.currentWeapon.PrimaryAttack();
        }
    }

    public void TakeDamage(int damageAmount, Canvas uiCanvas)
    {
        // Your logic here. For now, let's just reduce health.
        health -= damageAmount;

        // Add debugging


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
