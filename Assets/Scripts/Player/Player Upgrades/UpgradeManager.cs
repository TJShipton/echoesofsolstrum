using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    private List<IPlayerUpgrade> permanentUpgrades;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        permanentUpgrades = new List<IPlayerUpgrade>();
    }

    public void AddPermanentUpgrade(IPlayerUpgrade upgrade)
    {
        permanentUpgrades.Add(upgrade);
    }

    public void ApplyPermanentUpgrades(PlayerController player)
    {
        foreach (var upgrade in permanentUpgrades)
        {
            upgrade.ApplyUpgrade(player);
        }
    }
}
