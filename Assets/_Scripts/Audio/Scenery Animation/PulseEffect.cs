using SonicBloom.Koreo;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    public float maxScale = 1.5f; // Maximum scale of the pulse
    public float pulseDuration = 1f; // Duration of each pulse

    private Vector3 originalScale; // Original scale of the object
    private float pulseTimer = 0f; // Timer to track the pulse progress

    [EventID]
    public string pulseEventID = "Pulse";

    void Awake()
    {
        originalScale = transform.localScale;
        Koreographer.Instance.RegisterForEvents(pulseEventID, OnPulseTrigger);
    }

    void Update()
    {
        if (pulseTimer > 0)
        {
            // Decrease the timer
            pulseTimer -= Time.deltaTime;

            // Calculate the scale factor
            float scale = originalScale.x * (1 + Mathf.Sin((1 - (pulseTimer / pulseDuration)) * Mathf.PI) * (maxScale - 1));
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            // Reset to original scale when not pulsing
            transform.localScale = originalScale;
        }
    }

    void OnPulseTrigger(KoreographyEvent evt)
    {
        // Reset the pulse timer
        pulseTimer = pulseDuration;
    }
}
