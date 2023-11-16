using System;
using UnityEngine;

[Serializable]
public class ModchipInventoryItem : InventoryItem
{
    public GameObject modchipPrefab;
    public ModchipData modchipData;
    private GameObject player;
    private Transform modchipHolder;
    // Updated constructor that includes ModchipData as a parameter
    public ModchipInventoryItem(string id, GameObject modchipPrefab, ModchipData modchipData)
        : base(id, InventoryItemType.Modchip)
    {
        this.modchipPrefab = modchipPrefab;
        this.modchipData = modchipData; // Set the modchipData correctly
        this.icon = modchipData.modSprite; // Now this line should work fine

        // Find the player and modchipHolder
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                modchipHolder = playerController.modchipHolder.transform;
            }
        }


    }

    public void ActivateModchip()
    {
        if (modchipPrefab != null && modchipHolder != null)
        {
            GameObject instantiatedModchip = UnityEngine.Object.Instantiate(modchipPrefab, modchipHolder);
            instantiatedModchip.SetActive(true);
            // Additional activation logic if needed
        }
        else
        {
            Debug.LogError("Modchip prefab or holder is null.");
        }
    }


    // Method to deactivate the modchip
    public void Deactivate()
    {
        if (modchipPrefab != null)
        {
            modchipPrefab.SetActive(false);
            // Additional deactivation logic if needed
        }
    }

    public string GetDetails()
    {
        if (modchipData != null)
        {
            // Format the details string as required
            return $"Name: {modchipData.modchipName}\n" +
                   $"Decsription: {modchipData.modchipDescription}\n" +
                   $"Damage: {modchipData.modDamage}\n" +
                   $"Range: {modchipData.modRange}\n" +
                   $"Duration: {modchipData.modDuration} seconds\n" +
                   $"Cooldown: {modchipData.modCooldown} seconds";
        }
        else
        {
            return "Modchip data not available.";
        }
    }


}
