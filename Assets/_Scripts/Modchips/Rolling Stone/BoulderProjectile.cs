using UnityEngine;

public class BoulderProjectile : MonoBehaviour
{
    public ModchipData modchipData;
    public Canvas enemyCanvas;

    private void Start()
    {
        // Destroy the projectile after the duration specified in modchipData
        Destroy(gameObject, modchipData.modDuration);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(modchipData.modDamage, enemyCanvas);
                // Optionally, destroy the projectile on enemy hit
                // Destroy(gameObject);
            }
        }
    }

    public void SetModchipData(ModchipData data)
    {
        modchipData = data;
    }
}
