using UnityEngine;

public class ModchipPickup : MonoBehaviour
{
    public ModchipData modchipData; // Assign this in the inspector with your ModchipData ScriptableObject

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player GameObject has the "Player" tag
        {
            PickUp(other.gameObject); // Call the PickUp function
        }
    }

    private void PickUp(GameObject player)
    {
        InventoryManager inventoryManager = InventoryManager.instance;
        if (inventoryManager != null)
        {
            // Create a ModchipInventoryItem from the ModchipData
            ModchipInventoryItem modchipItem = new ModchipInventoryItem(modchipData.modchipName, gameObject, modchipData);

            // Call AddModchip method from InventoryManager
            inventoryManager.AddModchip(modchipItem);
            Debug.Log("Modchip Equipped");


            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null && playerController.modchipHolder != null)
            {
                // Use playerController.modchipHolder directly
                Instantiate(modchipData.modchipPrefab, playerController.modchipHolder.transform.position, playerController.modchipHolder.transform.rotation, playerController.modchipHolder.transform);
            }
            else
            {
                Debug.LogError("PlayerController or modchipHolder not found on the player.");
            }

            // Deactivate or destroy the pickup object
            gameObject.SetActive(false); // or Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventoryManager instance not found.");
        }
    }
}
