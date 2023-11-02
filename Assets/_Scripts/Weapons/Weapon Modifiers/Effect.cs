using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect

{
    public bool IsEffectEnded { get; protected set; }

    public abstract void StartEffect(Enemy enemy);
    public abstract void UpdateEffect(Enemy enemy);
    public abstract void EndEffect(Enemy enemy);
}