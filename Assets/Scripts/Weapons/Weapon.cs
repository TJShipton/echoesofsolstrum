using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon: MonoBehaviour
{
    public string weaponName;
    public int damage;
    public WeaponData weaponData;
    protected Animator weaponAnimator;

    public void InitWeaponAnimator(Animator animator)
    {
        this.weaponAnimator = animator;
    }

    public abstract void PrimaryAttack();

    //default attack logic
    public abstract void Upgrade();

    //upgrade logic
}

