using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int solCurrency;  // The amount of Sol currency the player has
    public int doubleJumpSolContributed = 0;  // Amount of Sol contributed towards DoubleJump
    public List<IPlayerUpgrade> availableUpgrades;
    public List<PermanentUpgradeButton> upgradeButtons;

    // Singleton instance
    public static CurrencyManager instance;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            SingletonManager.instance.RegisterSingleton(this); // Register with SingletonManager

            // Initialize available upgrades
            availableUpgrades = new List<IPlayerUpgrade>
            {
                new DoubleJumpUpgrade(),
                new BeefinessUpgrade(),
                new WeaponSlotUpgrade()
                // Add more upgrades here as you implement them
            };

            TestSolfatherSpawn.OnSolfatherSpawned += InitializeUpgradeButtons;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeUpgradeButtons(GameObject solfather)
    {
        upgradeButtons = new List<PermanentUpgradeButton>(solfather.GetComponentsInChildren<PermanentUpgradeButton>());

        if (upgradeButtons == null)
        {
            Debug.LogError("upgradeButtons is not initialized.");
            return;
        }

        // Assign upgrades to buttons
        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            if (i < upgradeButtons.Count)
            {
                if (upgradeButtons[i] != null)
                {
                    upgradeButtons[i].AssignUpgrade(availableUpgrades[i]);
                }
                else
                {
                    Debug.LogError($"upgradeButtons at index {i} is null.");
                }
            }
        }
    }

    // Function to add Sol
    public void AddSol(int amount)
    {
        solCurrency += amount;
        HudManager.instance.UpdateSolDisplay();  // Update the UI
    }

    // Function to spend Sol
    public bool SpendSol(int amount)
    {
        if (solCurrency >= amount)
        {
            solCurrency -= amount;
            HudManager.instance.OnSolChanged.Invoke();  // Inform the UIManager to update the Sol display
            return true;
        }
        else
        {
            Debug.Log("Not enough Sol");
            return false;
        }
    }

    public IPlayerUpgrade GetUpgradeByName(string name)
    {
        return availableUpgrades.Find(u => u.UpgradeName == name);
    }
}
