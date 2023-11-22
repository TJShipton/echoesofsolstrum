using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public SimpleMusicPlayer musicPlayer;
    public Koreography Layer1;
    public CompanionController CompanionController;  // assign your familiarController in the inspector

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SingletonManager.instance.RegisterSingleton(this); // Register with SingletonManager
        }
        else
        {
            Destroy(gameObject);
        }


    }



}


