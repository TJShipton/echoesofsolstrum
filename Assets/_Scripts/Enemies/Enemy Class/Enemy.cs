using System.Collections;
using System.Collections.Generic;
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
   
    public static Canvas enemyCanvas;
    public Animator Animator { get; private set; }
    public EnemyData Data => enemyData;  // Allows other scripts to access the enemyData

    public float LastAttackTime { get; set; }  // Tracks the last attack time

    private List<Effect> activeEffects = new List<Effect>();

    private void Start()
    {
        SetState(new IdleState(this));
        Animator = GetComponent<Animator>();

        // Initialize current health from enemyData
        currentHealth = enemyData.health;

        //Initialize enemy canvas
        if (enemyCanvas == null)
        {
            enemyCanvas = GameObject.Find("EnemyCanvas").GetComponent<Canvas>();
        }

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

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            Effect effect = activeEffects[i];
            effect.UpdateEffect(this);

            // Check if the effect has ended.
            if (effect.IsEffectEnded)
            {
                effect.EndEffect(this);
                activeEffects.RemoveAt(i);
            }
        }

    }

    public void SetState(EnemyState newState)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }

    // Method to add a modifier effect
    public void AddEffect(Effect effect)
    {
        activeEffects.Add(effect);
        effect.StartEffect(this);
    }

    // Method to remove a modifier effect
    public void RemoveEffect(Effect effect)
    {
        effect.EndEffect(this);
        activeEffects.Remove(effect);
    }


    public void TakeDamage(int damageAmount, Canvas enemyCanvas)
    {
        // Deduct the damage received
        currentHealth -= damageAmount;

        // Instantiate healthBarPrefab as a child of EnemyCanvas
        if (healthBarInstance == null)
        {
            // Initialize the EnemyCanvas if it's null
            if (enemyCanvas == null)
            {
                enemyCanvas = GameObject.Find("EnemyCanvas").GetComponent<Canvas>();
            }

            // Check if the healthBarPrefab is not null
            if (healthBarPrefab != null)
            {
                // Instantiate the healthBarPrefab as a new GameObject and set its parent
                if (enemyCanvas != null)
                {
                    healthBarInstance = Instantiate(healthBarPrefab, enemyCanvas.transform, false);
                }
                else
                {
                    Debug.LogWarning("EnemyCanvas is null, health bar will not be parented to it.");
                    healthBarInstance = Instantiate(healthBarPrefab);
                }
            }
            else
            {
                Debug.LogWarning("HealthBarPrefab is null, cannot create health bar.");
                return; // Return early if the prefab is not available.
            }

            healthBarSlider = healthBarInstance.GetComponent<Slider>();
            healthBarSlider.maxValue = enemyData.health;
            healthBarSlider.value = currentHealth;
        }
        else
        {
            healthBarSlider.value = currentHealth;
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