using UnityEngine;
using UnityEngine.SceneManagement;

public class HudCanvas : MonoBehaviour
{
    public static HudCanvas instance;
    //public Slider playerHealthBar; // Reference to health bar
    //public TextMeshProUGUI playerHealthText; // Reference to health text

    void Awake()
    {


        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            SingletonManager.instance.RegisterSingleton(this); // Register with SingletonManager
            SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas canvasComponent = GetComponent<Canvas>();

        if (scene.name == "Initialization")
        {
            canvasComponent.enabled = false;
        }
        else
        {
            canvasComponent.enabled = true;
        }
    }
}
