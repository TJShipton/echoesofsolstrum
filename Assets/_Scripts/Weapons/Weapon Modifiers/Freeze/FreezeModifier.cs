using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeModifier : IWeaponModifier
{
    public void ApplyEffect(Enemy enemy)
    {
        // Create a new instance of BurnEffect
       FreezeEffect freezeEffect = new FreezeEffect();

        // Apply it to the enemy
        enemy.AddEffect(freezeEffect);
    }

    public string GetName()
    {
        return "Freeze";
    }

    public string GetDescription()
    {
        return "";
    }

    public Sprite ModifierIcon
    {
        get { return Resources.Load<Sprite>("Path/To/BurnModifierIcon"); }
    }

}
