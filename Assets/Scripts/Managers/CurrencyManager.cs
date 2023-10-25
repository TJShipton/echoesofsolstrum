using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int solCurrency; // The amount of Sol currency the player has
    public int doubleJumpSolContributed = 0; // Amount of Sol contributed towards DoubleJump
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // Initialize available upgrades.
        availableUpgrades = new List<IPlayerUpgrade>
    {
        new DoubleJumpUpgrade(),
        new BeefinessUpgrade(),
        new WeaponSlotUpgrade()
        // Add more upgrades here as you implement them.
    };

        // Assign upgrades to buttons
        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            if (i < upgradeButtons.Count)
            {
                upgradeButtons[i].AssignUpgrade(availableUpgrades[i]);
            }
        }
    }


    // Function to add Sol
    public void AddSol(int amount)
    {
        solCurrency += amount;
        UIManager.instance.UpdateSolDisplay();  // Update the UI


    }



    // Function to spend Sol
    public bool SpendSol(int amount)
    {
        if (solCurrency >= amount)
        {

            solCurrency -= amount;
            UIManager.instance.OnSolChanged.Invoke();  // Inform the UIManager to update the Sol display
            //Debug.Log("Sol spent. Current Sol: " + solCurrency);



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