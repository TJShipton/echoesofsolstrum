
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgradeButton : MonoBehaviour
{
    public IPlayerUpgrade associatedUpgrade;  // Reference to the associated upgrade
    public TextMeshProUGUI upgradeNameText;  // UI Text for the upgrade's name
    public TextMeshProUGUI upgradeDescriptionText;  // UI Text for the upgrade description
    public TextMeshProUGUI totalCostText;
    public TextMeshProUGUI amountSpentText;
    public Slider progressBarFill;

    [Header("Sprite Information")]
    public string tickSpriteName = "Done";  // Name of the sprite in the TMP Sprite Asset


    [Header("Upgrade Details")]
    public string upgradeName;  // Name of the upgrade
    public string upgradeDescription;  // Description of the upgrade
    public int totalCost;  // Total cost of the upgrade

    private void Start()
    {
        // Set the name and description from the assigned values
        upgradeNameText.text = upgradeName;
        upgradeDescriptionText.text = upgradeDescription;
        totalCostText.text = totalCost.ToString();
        progressBarFill.maxValue = 1; // Setting max value to 1 since we're using a fraction
        progressBarFill.value = 0;
    }


    private void Update()
    {



    }


    // Method to assign upgrade
    public void AssignUpgrade(IPlayerUpgrade upgrade)
    {
        associatedUpgrade = upgrade;
        upgradeName = upgrade.UpgradeName;
        upgradeDescription = upgrade.UpgradeDescription;
        totalCost = upgrade.UpgradeCost;

        // Call the Start method to initialize the UI
        Start();
    }

    // Call this method when the button is clicked
    public void OnButtonClick()
    {
        int solToContribute = 1000; // Amount of Sol to contribute on each button click

        // Deduct sol from the player's total and add to amountSpent
        if (CurrencyManager.instance.SpendSol(solToContribute))
        {
            // Use the associatedUpgrade's AmountSpent property to track the total amount spent on the upgrade.
            associatedUpgrade.AmountSpent += solToContribute;
            UpdateProgressBar(solToContribute);

            // Check if the upgrade is fully funded and apply it
            if (associatedUpgrade.Progress >= 1)  // Progress is between 0 and 1
            {
                ApplyUpgrade();
            }
        }
    }

    void UpdateProgressBar(int amountSpent)
    {
        float fillAmount = associatedUpgrade.Progress;
        progressBarFill.value = fillAmount;

        if (fillAmount >= 1)  // If the upgrade is fully funded
        {
            // Display the tick sprite using the sprite's name in the TMP Sprite Asset
            amountSpentText.text = $"<sprite name={tickSpriteName}>";
        }
        else  // If the upgrade is not yet fully funded
        {
            amountSpentText.text = associatedUpgrade.AmountSpent.ToString();  // Display the amount spent
        }
    }


    void ApplyUpgrade()
    {
        // Obtain a reference to the PlayerController component
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // Use the associated upgrade's ApplyUpgrade method to apply the upgrade to the player
        associatedUpgrade.ApplyUpgrade(playerController);
        Debug.Log($"{upgradeName} Upgrade applied!");
    }
}
