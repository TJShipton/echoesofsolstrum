using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTest : MonoBehaviour
{
    public GameObject enemyPrefab; // Drag your 3DPrinterPrefab here in the inspector
    public Vector3 spawnPosition; // Position where Solfather will be spawned
    public Quaternion spawnRotation; // Rotation of the spawned Solfather

    public delegate void PlayerSpawned(GameObject player);
    public static event PlayerSpawned OnPlayerSpawned;

    void Start()
    {
        GameObject player = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
        OnPlayerSpawned?.Invoke(player);



    }
}
