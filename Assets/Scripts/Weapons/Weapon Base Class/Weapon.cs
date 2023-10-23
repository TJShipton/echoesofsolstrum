using UnityEngine;

public enum WeaponTier
{
    Common,
    Rare,
    Epic,
    Legendary
}

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public WeaponData weaponData;
    public Vector3 localOrientation;
    public Vector3 localPosition;
    protected Animator weaponAnimator;

    public void InitWeaponAnimator(Animator animator)
    {
        this.weaponAnimator = animator;
    }


    public string GetTierName()
    {
        return weaponData.weaponTier.ToString();
    }

    public void ApplyTierModifiers()
    {
        float damageMultiplier = 1.0f;
        switch (weaponData.weaponTier)
        {
            case WeaponTier.Common:
                damageMultiplier = weaponData.commonDamageMultiplier;
                break;
            case WeaponTier.Rare:
                damageMultiplier = weaponData.rareDamageMultiplier;
                break;
            case WeaponTier.Epic:
                damageMultiplier = weaponData.epicDamageMultiplier;
                break;
            case WeaponTier.Legendary:
                damageMultiplier = weaponData.legendaryDamageMultiplier;
                break;
        }

        weaponData.baseDamage = (int)(weaponData.originalBaseDamage * damageMultiplier);

        // ... other stat modifications ...
    }

    public virtual void ApplySpecialEffect()
    {
        if (weaponData.hasSpecialEffect)
        {
            // Apply the special effect based on weaponData.specialEffect
            // This is a placeholder, implement the specifics of each effect separately.
        }
    }




    public virtual void PrimaryAttack()
    {
        // Code for initiating the animation can go here later


    }


    //default attack logic


    public abstract void Upgrade();

    //upgrade logic


}

