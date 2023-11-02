public interface IPlayerUpgrade
{
    string UpgradeName { get; }

    string UpgradeDescription { get; }
    int UpgradeCost { get; }

    int AmountSpent { get; set; } // Added this
    float Progress { get; } // This gives a 0-1 progress value


    void ApplyUpgrade(PlayerController player);
}
