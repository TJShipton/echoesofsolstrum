using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

    private List<Effect> activeEffects = new List<Effect>();

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


    public void Unfreeze()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = false;

            // Reset animation to idle or default state
            if (Animator != null)
            {
            
                Animator.SetBool("IsWalking", true);
            }
        }
    }


    public void TakeDamage(int damageAmount, Canvas enemyCanvas)
    {
        // Deduct the damage received
        currentHealth -= damageAmount;

        // Instantiate healthBarPrefab as a child of EnemyCanvas if not already done
        if (healthBarInstance == null && healthBarPrefab != null)
        {
            if (EnemyCanvas.instance != null)
            {
                healthBarInstance = Instantiate(healthBarPrefab, EnemyCanvas.instance.transform, false);
                healthBarSlider = healthBarInstance.GetComponent<Slider>();
                healthBarSlider.maxValue = enemyData.health;
                healthBarSlider.value = currentHealth;
            }
            else
            {
                Debug.LogError("EnemyCanvas.instance is null, cannot instantiate health bar.");
            }
        }
        else if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("HealthBarSlider is null, cannot update health.");
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
