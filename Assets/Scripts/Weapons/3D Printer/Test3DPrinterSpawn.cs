using UnityEngine;

public class Test3DPrinterSpawn : MonoBehaviour
{
    public GameObject threeDPrinterPrefab; // Drag your 3DPrinterPrefab here in the inspector
    public Vector3 spawnLocation = new Vector3(0, 0, 0); // Set your desired spawn location

    void Start()
    {
        // Spawn the 3D printer at the specified location
        Instantiate(threeDPrinterPrefab, spawnLocation, Quaternion.identity);

        // Debug log to confirm that the 3D printer was spawned
       // Debug.Log($"Spawned 3D printer at position: {spawnLocation}");
    }
}
