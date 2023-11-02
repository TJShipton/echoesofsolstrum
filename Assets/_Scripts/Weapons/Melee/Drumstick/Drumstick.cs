using UnityEngine;

public class Drumstick : Weapon
{
    private Canvas EnemyCanvas;

    private int comboCounter = 0;
    private float lastAttackTime = 0;

    public float comboResetTime = 2.0f;
    public float spinAttackRadius = 2f;  // Radius for spin attack
    public LayerMask enemyLayers;

    

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
        weaponAnimator.SetTrigger("DrumstickAttack");

        //Start cooldown to deactivate weapon
        weaponData.StartCooldown();
    }

    private void DetectEnemiesInRadius()
    {
        Collider[] enemiesToDamage = Physics.OverlapSphere(transform.position, spinAttackRadius, enemyLayers);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            IDamageable enemyIDamageable = enemiesToDamage[i].GetComponent<IDamageable>();
            Enemy enemy = enemiesToDamage[i].GetComponent<Enemy>();  // Get the Enemy component
            if (enemyIDamageable != null)
            {
                enemyIDamageable.TakeDamage(weaponData.baseDamage, EnemyCanvas);

                // Apply weapon modifiers here
                foreach (var modifier in equippedModifiers)
                {
                    modifier.ApplyEffect(enemy);
                }
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
            if (equippedModifiers == null)
            {
                Debug.Log("equippedModifiers is null!");
            }
           
            IDamageable enemyIDamageable = other.GetComponent<IDamageable>();
            Enemy enemy = other.GetComponent<Enemy>();  // Get the Enemy component
            if (enemyIDamageable != null)
            {
                enemyIDamageable.TakeDamage(weaponData.baseDamage, EnemyCanvas);

                // Apply weapon modifiers here
                foreach (var modifier in equippedModifiers)
                {
                    modifier.ApplyEffect(enemy);
                }
            }
        }
    }



    private void Update()
    {
        if (comboCounter > 0 && Time.time - lastAttackTime > comboResetTime)
        {
            comboCounter = 0;
        }

        //Start cooldown to deactivtae weapon
        weaponData.UpdateCooldown(Time.deltaTime);

        if (!weaponData.IsOnCooldown() && gameObject.activeSelf)
        {
            gameObject.SetActive(false); // Deactivate the weapon when not on cooldown
        }


    }

   


}
