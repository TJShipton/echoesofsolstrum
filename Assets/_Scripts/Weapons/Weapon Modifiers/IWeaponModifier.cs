using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponModifier
{
    void ApplyEffect(Enemy enemy);
    string GetName();
}
