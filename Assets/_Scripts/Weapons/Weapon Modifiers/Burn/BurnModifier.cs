using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnModifier : IWeaponModifier
{
    public void ApplyEffect(Enemy enemy)
    {
        // Create a new instance of BurnEffect
        BurnEffect burnEffect = new BurnEffect();

        // Apply it to the enemy
        enemy.AddEffect(burnEffect);
    }

    public string GetName()
    {
        return "Burn";
    }

}
