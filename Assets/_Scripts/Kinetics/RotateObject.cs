using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Public variable to control rotation speed
    public float rotationSpeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its Y-axis at the specified speed
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
