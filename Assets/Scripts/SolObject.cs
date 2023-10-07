using UnityEngine;

public class SolObject : MonoBehaviour
{
    // Define how much Sol this object represents
    public int solValue = 1;

    // Function to be called when the player picks up this Sol object
    public void PickUpSol()
    {
        // Add the solValue to the player's sol
        GameManager.instance.AddSol(solValue);

        // Destroy this Sol object
        Destroy(gameObject);


    }
}

