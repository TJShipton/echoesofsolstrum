using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public string ItemId { get; set; }
    public Sprite icon;
    public virtual void Use() { }

    public InventoryItem(string id)
    {
        ItemId = id;
    }

}