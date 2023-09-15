using System.Collections;
using UnityEngine;
using SonicBloom.Koreo;

public class BeatFlasher : MonoBehaviour
{
    public string bassDrumEventID = "BassDrum";
   
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Koreographer.Instance.RegisterForEvents(bassDrumEventID, OnBeatDetected);
    }

    private void OnDisable()
    {
        if (Koreographer.Instance != null)
        {
            Koreographer.Instance.UnregisterForEvents(bassDrumEventID, OnBeatDetected);
        }
        Koreographer.Instance.UnregisterForEvents(bassDrumEventID, OnBeatDetected);
    }

    private void OnBeatDetected(KoreographyEvent koreoEvent)
    {
        StartCoroutine(FlashSprite());
    }

    private IEnumerator FlashSprite()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // Set alpha to 0 to make sprite transparent
        
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 second
        spriteRenderer.color = Color.white; // Set color back to flashColor
        
    }


}
