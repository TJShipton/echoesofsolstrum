using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string LastInputMethod { get; set; } = "keyboard";  // Property to store last input method
    private float joystickTimer = 0f;  // Timer to track joystick input
    private float joystickThreshold = 0.4f;  // Time in seconds to wait before setting to "controller"

    


    void Awake()
    {

        // Singleton pattern
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

   
    }


    private void Update()
    {
        // Detect last input method
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown ||
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            LastInputMethod = "keyboard";
            joystickTimer = 0f;  // Reset the joystick timer
        }

        // Detect joystick button press
        if (Input.GetButtonDown("Fire2"))
        {
            LastInputMethod = "controller";
        }

        // Detect joystick movement
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        // Check if there is significant movement in either axis
        if (Mathf.Abs(horizontalAxis) > 0.1f || Mathf.Abs(verticalAxis) > 0.1f)
        {
            joystickTimer += Time.deltaTime;  // Increment the joystick timer
            if (joystickTimer >= joystickThreshold)
            {
                LastInputMethod = "controller";
            }
        }
        else
        {
            joystickTimer = 0f;  // Reset the joystick timer
        }
    }


    





}
