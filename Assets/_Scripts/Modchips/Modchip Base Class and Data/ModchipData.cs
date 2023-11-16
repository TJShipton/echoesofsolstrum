using UnityEngine;

[CreateAssetMenu(fileName = "NewModchip", menuName = "Modchips/Modchip Data")]

public class ModchipData : ScriptableObject
{
    public GameObject modchipPrefab;
    public string modchipName;
    public string modchipDescription;
    public int modDamage;
    public int modRange;
    public float modDuration;
    public float modCooldown;
    private float currentModCooldownTime;
    public Sprite modSprite;



}
