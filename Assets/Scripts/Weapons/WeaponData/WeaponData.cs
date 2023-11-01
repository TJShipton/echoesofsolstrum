using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponTier weaponTier;
    public string weaponDescription;
    public int baseDamage;
    public int originalBaseDamage;
    public int range;
    public int speed;

    public GameObject projectilePrefab;

    public int projectileSpeed;



    public float commonDamageMultiplier = 1.0f;
    public float rareDamageMultiplier = 1.2f;
    public float epicDamageMultiplier = 1.5f;
    public float legendaryDamageMultiplier = 2.0f;


  

    public Sprite iconSprite;
    // Other properties can be added


    private void OnEnable()
    {
        // Reset baseDamage to the original value whenever this WeaponData object is enabled
        baseDamage = originalBaseDamage;
    }

   

}
