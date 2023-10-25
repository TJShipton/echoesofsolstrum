using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyData enemyData;  // Serialized for the editor
    private EnemyState currentState;
    private float currentHealth;

    [SerializeField]
    private GameObject healthBarPrefab;  // Drag your HealthBar prefab here from Unity Editor
    private GameObject healthBarInstance;
    private Slider healthBarSlider;

    public Animator Animator { get; private set; }
    public EnemyData Data => enemyData;  // Allows other scripts to access the enemyData

    public float LastAttackTime { get; set; }  // Tracks the last attack time

    private void Start()
    {
        SetState(new IdleState(this));
        Animator = GetComponent<Animator>();

        // Initialize current health from enemyData
        currentHealth = enemyData.health;
    }

    private void Update()
    {
        if (currentState != null)
            currentState.UpdateState();

        // Update health bar position to follow enemy
        if (healthBarInstance != null)
        {
            Vector3 healthBarPos = transform.position;
            healthBarPos.y += 2;  // Adjust as needed
            healthBarInstance.transform.position = healthBarPos;
        }
    }

    public void SetState(EnemyState newState)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }

    public void TakeDamage(int damageAmount, Canvas EnemyCanvas)
    {
        //Debug.Log("TakeDamage called. Damage Amount: " + damageAmount); // Debug 1
        //Debug.Log("Provided Canvas: " + (EnemyCanvas == null ? "Null" : "Exists")); // Debug 2

        currentHealth -= damageAmount; // Deduct the damage received

        if (healthBarInstance == null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, EnemyCanvas.transform);
            healthBarSlider = healthBarInstance.GetComponent<Slider>();
            healthBarSlider.maxValue = enemyData.health;
            healthBarSlider.value = currentHealth;

            //Debug.Log("Health Bar Instance Created."); // Debug 3
        }
        else
        {
            healthBarSlider.value = currentHealth;
            //Debug.Log("Health Bar Updated. Current Value: " + healthBarSlider.value); // Debug 4
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }



    private void Die()
    {
        // Perform death logic here
        // For example: Animator.SetTrigger("Die");
        Debug.Log("Enemy died.");

        // Destroy the health bar
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance);
        }

        // Destroy the enemy GameObject
        Destroy(gameObject);
    }
}