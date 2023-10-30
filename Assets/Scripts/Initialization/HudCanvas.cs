using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HudCanvas : MonoBehaviour
{
    public static HudCanvas instance;
    //public Slider playerHealthBar; // Reference to health bar
    //public TextMeshProUGUI playerHealthText; // Reference to health text

    void Awake()
    {
        Debug.Log("HudCanvas Awake Called");

        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
