using UnityEngine;

public class HudCanvas : MonoBehaviour
{
    public static HudCanvas instance;

    void Awake()
    {
        //Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the sceneLoaded event
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    Canvas canvasComponent = GetComponent<Canvas>();

    //    if (scene.name == "Initialization")
    //    {
    //        canvasComponent.enabled = false;
    //    }
    //    else
    //    {
    //        canvasComponent.enabled = true;
    //    }
    //}
}
