using SonicBloom.Koreo;
using UnityEngine;

public class KoreographerColorTrigger : MonoBehaviour
{
    public EmissiveColorChanger colorChanger; // Reference to the EmissiveColorChanger script
    public string eventID; // The ID of the Koreography event to listen for

    void Start()
    {
        // Register to Koreographer Event with the specified eventID
        Koreographer.Instance.RegisterForEvents(eventID, OnKoreographyEvent);
    }

    void OnDestroy()
    {
        // Make sure to unregister the event when this object is destroyed
        if (Koreographer.Instance != null)
        {
            Koreographer.Instance.UnregisterForEvents(eventID, OnKoreographyEvent);
        }
    }

    void OnKoreographyEvent(KoreographyEvent koreoEvent)
    {
        // Call the function in EmissiveColorChanger to trigger the color change
        // You can modify this part to pass any specific parameters you need
        colorChanger.TriggerColorChange();
    }
}
