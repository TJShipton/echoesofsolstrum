using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager instance;

    private List<Object> registeredSingletons = new List<Object>();

    void Awake()
    {
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

    public void RegisterSingleton(Object singleton)
    {
        if (!registeredSingletons.Contains(singleton))
        {
            registeredSingletons.Add(singleton);
        }
    }


}
