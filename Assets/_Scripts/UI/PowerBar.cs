using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public Slider powerBar; // the power bar slider
    public int maxHits; // maximum number of hits to fill the bar
    private int currentHits; // current number of hits
    private bool powerBarFull = false; // track if power bar is full

    void Start()
    {
        currentHits = 0;
        powerBar.value = 0;
    }

    void Update()
    {
        // If power bar is full and player presses Space, launch attack
        if (powerBarFull && Input.GetButtonDown("Fire3"))
        {
            LaunchSpecialAttack();
        }
    }

    public void AddHit()
    {
        if (powerBarFull)
            return; // if power bar is full, do not accept more hits

        currentHits++;
        powerBar.value = (float)currentHits / maxHits;

        if (currentHits >= maxHits)
        {
            powerBarFull = true; // power bar is now full
        }
    }

    void LaunchSpecialAttack()
    {
        // For testing, print a message to the Unity console
        Debug.Log("Special Attack Launched!");

        currentHits = 0; // reset hits count
        powerBar.value = 0; // reset power bar
        powerBarFull = false; // power bar is not full anymore
    }
}
