using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestPlayerSpawn : MonoBehaviour
{
     public GameObject playerPrefab; // Drag your 3DPrinterPrefab here in the inspector
    public Vector3 spawnPosition; // Position where Solfather will be spawned
    public Quaternion spawnRotation; // Rotation of the spawned Solfather

    public delegate void PlayerSpawned(GameObject player);
    public static event PlayerSpawned OnPlayerSpawned;

    void Start()
    {
        GameObject player = Instantiate(playerPrefab, spawnPosition, spawnRotation);
        OnPlayerSpawned?.Invoke( player );

        CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
        vcam.Follow = player.transform;
        
    }
}
