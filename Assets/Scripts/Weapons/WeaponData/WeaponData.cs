using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponTier weaponTier;
    public int baseDamage;
    public int originalBaseDamage;
    //public int upgradedDamage;
    public int range;
    public int speed;

    public GameObject projectilePrefab;

    public int projectileSpeed;



    public float commonDamageMultiplier = 1.0f;
    public float rareDamageMultiplier = 1.2f;
    public float epicDamageMultiplier = 1.5f;
    public float legendaryDamageMultiplier = 2.0f;

    public bool hasSpecialEffect;
    public SpecialEffect specialEffect;

    public Sprite iconSprite;
    // Other properties can be added


    private void OnEnable()
    {
        // Reset baseDamage to the original value whenever this WeaponData object is enabled
        baseDamage = originalBaseDamage;
    }

    public string GetFormattedInfo()
    {
        return $"Name: {weaponName}\nDamage: {baseDamage}\nRange: {range}\nSpeed: {speed}";
    }



    [System.Serializable]
    public class SpecialEffect
    {
        public EffectType effectType;
        public float effectStrength;
        // ... other effect properties ...
    }

    public enum EffectType
    {
        None,
        Freeze,
        Burn,
        // ... other effect types ...
    }


}
