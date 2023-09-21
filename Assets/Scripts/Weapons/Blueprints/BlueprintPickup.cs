using UnityEngine;

public class BlueprintPickup : MonoBehaviour
{
    public WeaponBlueprint blueprint; // The blueprint this pickup represents

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            Debug.Log("Player tag detected"); // Log for debugging
            GameManager gameManager = FindObjectOfType<GameManager>();

            if (gameManager == null)
            {
                Debug.LogError("GameManager not found in the scene!"); // Error log if GameManager is not found
            }
            else
            {
                Debug.Log("GameManager found: " + (gameManager != null)); // Log for debugging

                // Call UnlockWeaponFromBlueprint instead of UnlockWeapon
                gameManager.UnlockWeaponFromBlueprint(blueprint);
            }

            Destroy(gameObject);  // Remove the blueprint object from the scene
            Debug.Log("Blueprint Picked Up: " + blueprint.weaponName); // Log for debugging
        }
    }
}
