using System;
using UnityEngine;

[Serializable]
public class WeaponInventoryItem : InventoryItem
{
    public GameObject weaponPrefab;

    public WeaponInventoryItem(string id, GameObject weaponPrefab) : base(id)
    {
        this.weaponPrefab = weaponPrefab;
    }

    public override void Use()
    {



    }
}



