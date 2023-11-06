using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    public static EnemyCanvas instance;

    private void Awake()
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
}