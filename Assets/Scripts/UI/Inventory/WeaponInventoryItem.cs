using System;
using UnityEngine;

[Serializable]
public class WeaponInventoryItem : InventoryItem
{
    public GameObject weaponPrefab;

    public WeaponInventoryItem(string id) : base(id)
    {


    }
    public override void Use()
    {
        WeaponManager.instance.AddWeapon(weaponPrefab.GetComponent<Weapon>());


    }
}



