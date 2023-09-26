
using UnityEngine;
using UnityEngine.UI;
public class MeleeHitbox : MonoBehaviour
{
    [SerializeField]
    private Weapon parentWeapon; // Assuming you have some parent Weapon script that holds damage info


    [SerializeField]
    private Canvas uiCanvas; // Drag your UI canvas here from the Unity Editor

    void Start()
    {
        parentWeapon = GetComponentInParent<Weapon>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(parentWeapon.damage, uiCanvas);  // Apply the damage here and pass in the canvas
                Debug.Log("Damage is being applied through MeleeHitbox");  // Debug line to confirm this is being called
            }
        }
    }
}
