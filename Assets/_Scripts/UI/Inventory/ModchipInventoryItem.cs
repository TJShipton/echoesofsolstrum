using System;
using UnityEngine;

[Serializable]
public class ModchipInventoryItem : InventoryItem
{
    public GameObject modchipPrefab;
    public ModchipData modchipData;
    private GameObject player;

    // Updated constructor that includes ModchipData as a parameter
    public ModchipInventoryItem(string id, GameObject modchipPrefab, ModchipData modchipData)
        : base(id, InventoryItemType.Modchip)
    {
        this.modchipPrefab = modchipPrefab;
        this.modchipData = modchipData; // Set the modchipData correctly
        this.icon = modchipData.modSprite; // Now this line should work fine
        player = GameObject.FindGameObjectWithTag("Player");


    }

    public void Activate()
    {
        if (modchipPrefab != null)
        {
            GameObject instantiatedModchip = UnityEngine.Object.Instantiate(modchipPrefab, player.transform);
            instantiatedModchip.SetActive(true);
            // Additional activation logic if needed
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




}
