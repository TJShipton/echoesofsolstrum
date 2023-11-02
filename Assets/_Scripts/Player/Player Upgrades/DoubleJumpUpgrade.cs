using UnityEngine;

public class DoubleJumpUpgrade : IPlayerUpgrade
{
    public string UpgradeName => "Double Jump";

    public string UpgradeDescription => "Hendric can jump one more time mid-air";
    public int UpgradeCost => 1000;

    public int AmountSpent { get; set; }

    public float Progress => (float)AmountSpent / UpgradeCost;
    public void ApplyUpgrade(PlayerController player)
    {
        player.EnableDoubleJump();
        Debug.Log("Double Jump Upgrade applied!");  // Debugging to confirm application of the upgrade
    }
}