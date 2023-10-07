using UnityEngine;
public class MeleeHitbox : MonoBehaviour
{
    [SerializeField]
    private Weapon parentWeapon; // Assuming you have some parent Weapon script that holds damage info


    [SerializeField]
    private Canvas uiCanvas;

    void Start()
    {
        parentWeapon = GetComponentInParent<Weapon>();

      

    }
    //Apply damage to enemy
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null && parentWeapon != null)  // Make sure parentWeapon is set
            {
                enemy.TakeDamage(parentWeapon.weaponData.baseDamage, uiCanvas);

            }
        }
    }
}
