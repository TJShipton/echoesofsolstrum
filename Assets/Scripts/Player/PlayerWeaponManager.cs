using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class PlayerWeaponManager : MonoBehaviour
{
    public InventorySlot[] weaponSlots = new InventorySlot[2];
    public int activeWeaponSlot = 0;
    public bool hasWeaponUpgrade = false;



    //public void SwitchWeaponSlot()
    //{
    //    if (hasWeaponUpgrade)
    //    {
    //        activeWeaponSlot = 1 - activeWeaponSlot;
    //    }
    //}

    //public void EnableWeaponUpgrade()
    //{
    //    hasWeaponUpgrade = true;
    //}
}
