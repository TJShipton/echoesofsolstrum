using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;  // Make sure this is serialized for the editor
    private EnemyState currentState;

    public Animator Animator { get; private set; }
    public EnemyData Data => enemyData;  // This allows other scripts to access the enemyData

    public float LastAttackTime { get; set; }  // Track the last attack time

   

    private void Start()
    {
        // Initialize to idle state for this example
        SetState(new IdleState(this));
        Animator = GetComponent <Animator>();
    }

    private void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }


    public void SetState(EnemyState newState)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }
}
