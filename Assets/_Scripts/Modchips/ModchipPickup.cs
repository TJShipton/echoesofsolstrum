using UnityEngine;

public class ModchipPickup : MonoBehaviour
{
    public ModchipData modchipData; // Assign this in the inspector with your ModchipData ScriptableObject
    private bool isPickedUp = false;
    private void OnTriggerEnter(Collider other)
    {
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


            // Deactivate or destroy the pickup object
            gameObject.SetActive(false); // or Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventoryManager instance not found.");
        }
    }
}
