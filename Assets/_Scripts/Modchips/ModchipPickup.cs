using TMPro;
using UnityEngine;

public class ModchipPickup : MonoBehaviour
{
    public ModchipData modchipData; // Assign this in the inspector with your ModchipData ScriptableObject
    private bool isPickedUp = false;
    public TextMeshProUGUI modchipUnlockText;
    public float modchipUnlockDisplayDuration = 2.0f;

    private void Start()
    {
        modchipUnlockText = HudManager.instance.GetModchipUnlockText();

        if (modchipUnlockText == null)
        {
            Debug.LogError("Unlock Text not assigned!");
        }
        else
        {
            modchipUnlockText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HideModchipObject();

        if (!isPickedUp && other.CompareTag("Player"))
        {
            isPickedUp = true;
            PickUp(other.gameObject);
        }
    }
    private void PickUp(GameObject player)
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        if (inventoryManager != null)
        {
            // Create a ModchipInventoryItem from the ModchipData
            ModchipInventoryItem modchipItem = new ModchipInventoryItem(modchipData.modchipName, gameObject, modchipData);

            // Add the modchip to the general modchip inventory, not directly equip it
            inventoryManager.AddModchip(modchipItem);
            Debug.Log("Modchip added to inventory");

            ShowModchipUnlockText($"{modchipData.modchipName} Modchip Unlocked");

        }
        else
        {
            Debug.LogError("InventoryManager instance not found.");
        }
    }

    private void ShowModchipUnlockText(string message)
    {
        if (modchipUnlockText != null)
        {
            modchipUnlockText.text = message;
            modchipUnlockText.gameObject.SetActive(true);

            StartCoroutine(HideModchipUnlockTextAfterDelay());
        }
        else
        {
            Debug.LogError("Unlock Text not assigned!");
        }
    }

    private System.Collections.IEnumerator HideModchipUnlockTextAfterDelay()
    {

        yield return new WaitForSeconds(modchipUnlockDisplayDuration);

        if (modchipUnlockText != null)
        {
            modchipUnlockText.gameObject.SetActive(false);

        }
        HideModchipObject();



    }

    private void HideModchipObject()
    {
        // Disable all the renderers in the Blueprint object and its children
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        // Disable the collider so it can't be interacted with again
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

}
