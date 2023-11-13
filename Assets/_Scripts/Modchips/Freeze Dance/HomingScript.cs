using UnityEngine;

public class HomingScript : MonoBehaviour
{
    public GameObject target;
    public float homingDuration = 2f;
    public float homingStrength = 1f; // New variable for homing strength
    public Vector3 force;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (target != null && (Time.time - startTime) < homingDuration)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Vector3 newVelocity = direction * force.magnitude * homingStrength; // Apply homing strength
            GetComponent<Rigidbody>().velocity = newVelocity;
        }
    }
}
