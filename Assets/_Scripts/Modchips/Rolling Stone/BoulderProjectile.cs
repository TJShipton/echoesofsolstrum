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

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(modchipData.modDamage, enemyCanvas);
        }
        // Optionally, you can destroy the projectile on collision
        // Destroy(gameObject);
    }

    public void SetModchipData(ModchipData data)
    {
        modchipData = data;
    }
}
