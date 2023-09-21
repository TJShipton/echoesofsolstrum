using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // The target the camera will follow (your player)
    public Vector3 offset;    // Offset position of the camera
    public float smoothSpeed; // How smoothly the camera will catch up

    // Start is called before the first frame update
    void Start()
    {
        // Optional: Initialize your variables here, if needed
    }

    // LateUpdate is called once per frame, but after all Update methods have been called
    void LateUpdate()
    {
        // Calculate the desired position based on target and offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Update the camera position
        transform.position = smoothedPosition;

        // Debug Log to see the current camera position
        Debug.Log("Camera position: " + transform.position);
    }
}
