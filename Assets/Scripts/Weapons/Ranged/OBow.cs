public class OBow : Weapon
{
    // O-Bow specific properties
    private bool isRanged = true;  // To switch between ranged and melee
    private bool isUpgraded = false;

    public override void PrimaryAttack()
    {
        if (isRanged)
        {
            if (!isUpgraded)
            {
                // Basic ranged attack logic
            }
            else
            {
                // Harmonize attack logic
            }
        }
        else
        {
            // Melee attack logic
        }
    }

    public override void Upgrade()
    {
        isUpgraded = true;
    }

    public void ToggleRangedMode(bool ranged)
    {
        isRanged = ranged;
    }
}

