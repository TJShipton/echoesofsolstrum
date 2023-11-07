using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponModifier
{
    string GetName();
    //string GetDescription();
    
    Sprite ModifierIcon { get; }

    void ApplyEffect(Enemy enemy);
    
}
