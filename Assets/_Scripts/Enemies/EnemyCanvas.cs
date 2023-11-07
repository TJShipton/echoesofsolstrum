using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    public static EnemyCanvas instance;

    private void Awake()
    {

        if (instance != null && instance != this)
        {

            Destroy(gameObject);
        }
        else
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }



}