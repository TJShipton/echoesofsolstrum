using UnityEngine;

public class BeefinessUpgrade : IPlayerUpgrade

{
    public string UpgradeName => "Beefiness";

    public string UpgradeDescription => "Increases overall health pool by 15%";
    public int UpgradeCost => 1500;

    public int AmountSpent { get; set; }

    public float Progress => (float)AmountSpent / UpgradeCost;
    public void ApplyUpgrade(PlayerController player)
    {
        int additionalHealth = Mathf.RoundToInt(player.maxHealth * 0.15f);
        player.maxHealth += additionalHealth;
        player.currentHealth += additionalHealth;

        // Update the health UI to reflect the new health values
        player.UpdateHealthUI();

        Debug.Log("Beefiness Upgrade applied! New max health: " + player.maxHealth);
    }
}