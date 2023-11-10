using UnityEngine;

public class ModchipSpawnTest : MonoBehaviour
{
    public GameObject modchipPrefab; // Drag your 3DPrinterPrefab here in the inspector
    public Vector3 spawnLocation = new Vector3(1, 0, 0); // Set your desired spawn location

    void Start()
    {
        // Spawn the 3D printer at the specified location
        Instantiate(modchipPrefab, spawnLocation, Quaternion.identity);

        // Debug log to confirm that the 3D printer was spawned
        // Debug.Log($"Spawned 3D printer at position: {spawnLocation}");
    }
}
