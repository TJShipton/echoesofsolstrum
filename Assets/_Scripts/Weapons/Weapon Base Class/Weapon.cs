using System.Collections.Generic;
using System.Linq;
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

    public List<IWeaponModifier> equippedModifiers;

    List<IWeaponModifier> possibleModifiers = new List<IWeaponModifier>
    {
         new BurnModifier(),
         //new FreezeModifier(),
          // Add other modifiers RARE TIER here
    };


    //List<IWeaponModifier> possibleEpicModifiers = new List<IWeaponModifier>
    //{
    //     new FreezeModifier(),
    //};  
    
    private void Awake()
    {
        // Initialize the equippedModifiers list
        equippedModifiers = new List<IWeaponModifier>();
    }

    void Start()
    {
       

       
    }


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

    public void EquipRandomModifiers()
    {
        if (equippedModifiers == null)
        {
            equippedModifiers = new List<IWeaponModifier>();
        }

        int numberOfModifiers = 0;

        switch (weaponData.weaponTier)
        {
            case WeaponTier.Common:
                numberOfModifiers = 0;
                break;
            case WeaponTier.Rare:
                numberOfModifiers = 1;
                break;
            case WeaponTier.Epic:
                numberOfModifiers = 2;
                break;
            case WeaponTier.Legendary:
                numberOfModifiers = 3;
                break;
        }

        //Debug.Log($"Weapon Tier: {weaponData.weaponTier}, Intended Number of Modifiers: {numberOfModifiers}");

        List<IWeaponModifier> remainingModifiers = new List<IWeaponModifier>(possibleModifiers);

        for (int i = 0; i < numberOfModifiers; i++)
        {
            if (remainingModifiers.Count == 0)
            {
                //Debug.LogWarning("No more modifiers to add.");
                break;
            }

            int randomIndex = UnityEngine.Random.Range(0, remainingModifiers.Count);
            IWeaponModifier selectedModifier = remainingModifiers[randomIndex];

            // Check if a modifier of this type is already equipped
            if (!equippedModifiers.Any(em => em.GetType() == selectedModifier.GetType()))
            {
                equippedModifiers.Add(selectedModifier);
            }
            else
            {
                //Debug.LogWarning($"Modifier of type {selectedModifier.GetType().Name} is already equipped.");
            }

            remainingModifiers.RemoveAt(randomIndex);
        }

        //Debug.Log($"Actual Number of Modifiers: {equippedModifiers.Count}, Modifiers: {string.Join(", ", equippedModifiers.Select(m => m.GetType().Name))}");
    }






    public virtual void PrimaryAttack()
    {
        
    }


    public void DeactivateWeapon()
    {
        // Deactivate the weapon
        gameObject.SetActive(false);
    }

}

