using UnityEngine;

public class SystemManager : MonoBehaviour
{
    private static SystemManager _instance;

    public static SystemManager Instance
    {
        get { return _instance; }
    }

    public HudManager hudManager; // Drag your HudManager here in Inspector
    public HudCanvas hudCanvas;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
