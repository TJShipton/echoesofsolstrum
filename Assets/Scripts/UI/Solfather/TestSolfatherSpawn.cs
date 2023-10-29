using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSolfatherSpawn : MonoBehaviour
{
    public GameObject solfatherPrefab; // Drag your 3DPrinterPrefab here in the inspector
    public Vector3 spawnPosition; // Position where Solfather will be spawned
    public Quaternion spawnRotation; // Rotation of the spawned Solfather

    public delegate void SolfatherSpawned(GameObject solfather);
    public static event SolfatherSpawned OnSolfatherSpawned;

    void Start()
    {
        GameObject solfather = Instantiate(solfatherPrefab, spawnPosition, spawnRotation);
        OnSolfatherSpawned?.Invoke(solfather);
    }
}
