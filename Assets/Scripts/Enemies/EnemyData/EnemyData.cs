using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float speed;
    public float chaseRange;
    public float attackRange;
    public int attackDamage;
    public float attackDelay;
}

