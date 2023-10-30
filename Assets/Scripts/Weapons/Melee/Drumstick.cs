using UnityEngine;

public class Drumstick : Weapon
{
    public Canvas EnemyCanvas;

    private int comboCounter = 0;
    private float lastAttackTime = 0;

    public float comboResetTime = 2.0f;
    public float spinAttackRadius = 2f;  // Radius for spin attack
    public LayerMask enemyLayers;

    public Animator animator;

    void Start()
    {
        // Access the GameManager to get the EnemyCanvas
        if (EnemyCanvas == null)
        {
            EnemyCanvas = GameManager.EnemyCanvas;
        }
    }

    public override void PrimaryAttack()
    {
        comboCounter++;
        lastAttackTime = Time.time;

        switch (comboCounter)
        {
            case 1:
                Debug.Log("Combo 1st hit!");
                break;
            case 2:
                Debug.Log("Combo 2nd hit!");
                break;
            case 3:
                Debug.Log("Paradiddle spin attack!");
                comboCounter = 0;
                DetectEnemiesInRadius();
                break;
            default:
                comboCounter = 0;
                break;
        }
            animator.SetTrigger("DrumstickAttack");

    }

    private void DetectEnemiesInRadius()
    {
        Collider[] enemiesToDamage = Physics.OverlapSphere(transform.position, spinAttackRadius, enemyLayers);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            IDamageable enemy = enemiesToDamage[i].GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.baseDamage, EnemyCanvas);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.baseDamage, EnemyCanvas);
            }
        }
    }

    private void Update()
    {
        if (comboCounter > 0 && Time.time - lastAttackTime > comboResetTime)
        {
            comboCounter = 0;
        }
    }
}
