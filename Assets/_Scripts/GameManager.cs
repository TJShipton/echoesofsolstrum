using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string LastInputMethod { get; set; } = "keyboard";  // Property to store last input method

    public int sol = 0;

    private float joystickTimer = 0f;  // Timer to track joystick input
    private float joystickThreshold = 0.5f;  // Time in seconds to wait before setting to "controller"

    public static Canvas EnemyCanvas;

    [SerializeField]
    private TextMeshProUGUI weaponUnlockText; //weapon unlock text 

    //public List<WeaponModifier> allPossibleModifiers;



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

        EnemyCanvas = FindObjectOfType<Canvas>();

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
        for (int i = 0; i < 20; i++)  // Iterate through 20 possible joystick buttons
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
                LastInputMethod = "controller";
                break;
            }
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

    public TextMeshProUGUI GetUnlockText()
    {
        return weaponUnlockText;
    }




}
