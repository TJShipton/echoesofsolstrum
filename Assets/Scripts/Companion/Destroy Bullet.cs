using UnityEngine;



public class BulletChecker : MonoBehaviour
{
    private void Update()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Check if the bullet is out of camera view
        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            Destroy(gameObject);
        }
    }
}
