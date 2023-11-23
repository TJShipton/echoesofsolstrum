using System;
using UnityEngine;

[Serializable]
public class ModchipInventoryItem : InventoryItem
{
    public GameObject modchipPrefab;
    public ModchipData modchipData;
    private GameObject modchipInstance;
    private GameObject player;
    private Transform modchipHolder;

    public ModchipInventoryItem(string id, GameObject modchipPrefab, ModchipData modchipData)
        : base(id, InventoryItemType.Modchip)
    {
        this.modchipPrefab = modchipPrefab;
        this.modchipData = modchipData; // Set the modchipData 
        this.icon = modchipData.modSprite;



    }

    public void ActivateModchip(GameObject targetHolder)
    {
        if (modchipPrefab != null && targetHolder != null)
        {
            if (modchipInstance != null)
            {
                UnityEngine.Object.Destroy(modchipInstance);
            }

            modchipInstance = UnityEngine.Object.Instantiate(modchipPrefab, targetHolder.transform);
            modchipInstance.transform.localPosition = Vector3.zero; // Reset local position
            modchipInstance.transform.localRotation = Quaternion.identity; // Reset local rotation
            modchipInstance.SetActive(true);
        }
        else
        {
            Debug.LogError("Modchip prefab or holder is null.");
        }
    }


    public Modchip GetModchip()
    {
        return modchipInstance != null ? modchipInstance.GetComponent<Modchip>() : null;
    }


    // Method to deactivate the modchip
    public void Deactivate()
    {
        if (modchipPrefab != null)
        {
            modchipPrefab.SetActive(false);

        }
    }

    public string GetDetails()
    {
        if (modchipData != null)
        {
            // Format the details  
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
