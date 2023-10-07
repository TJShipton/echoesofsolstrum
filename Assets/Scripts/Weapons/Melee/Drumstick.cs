using UnityEngine;

public class Drumstick : Weapon
{
    // Drumstick specific properties
    private bool isUpgraded = false;
    private int comboCounter = 0;
    private float lastAttackTime = 0;

    public float comboResetTime = 2.0f;
    public float spinAttackRadius = 2f;  // Radius for spin attack
    public LayerMask enemyLayers;

    private Animator playerAnimator;

    [SerializeField]
    private Canvas EnemyCanvas;

    void Start()
    {
        
      
    }


    public override void PrimaryAttack()
    {
        // weaponAnimator.SetTrigger("Attack");  // Trigger the attack animation

        if (!isUpgraded)
        {
            BasicAttack();


        }
        else
        {
            ComboAttack();
        }
    }

    private void BasicAttack()
    {

        // Animate, sound effects, etc.
    }

    private void ComboAttack()
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
    }



    private void DetectEnemiesInRadius()
    {
        Collider[] enemiesToDamage = Physics.OverlapSphere(transform.position, spinAttackRadius, enemyLayers);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            IDamageable enemy = enemiesToDamage[i].GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(weaponData.baseDamage, EnemyCanvas); // Passing EnemyCanvas

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

    public override void Upgrade()
    {
        isUpgraded = true;
    }
}