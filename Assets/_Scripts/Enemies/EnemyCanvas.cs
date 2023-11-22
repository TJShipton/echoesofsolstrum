using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    public static EnemyCanvas instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

    }



}