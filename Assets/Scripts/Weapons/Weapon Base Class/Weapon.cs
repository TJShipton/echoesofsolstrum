using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    //public int damage;
    public WeaponData weaponData;
    public Vector3 localOrientation;
    public Vector3 localPosition;
    protected Animator weaponAnimator;

    public void InitWeaponAnimator(Animator animator)
    {
        this.weaponAnimator = animator;
    }

    public virtual void PrimaryAttack()
    {
        // Code for initiating the animation can go here later

       
    }
   

    //default attack logic
    public abstract void Upgrade();

    //upgrade logic
}

