using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    public static EnemyCanvas instance;

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