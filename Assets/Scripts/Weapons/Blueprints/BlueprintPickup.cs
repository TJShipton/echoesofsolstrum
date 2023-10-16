using UnityEngine;

public class BlueprintPickup : MonoBehaviour
{
    public WeaponBlueprint blueprint; // The blueprint this pickup represents

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {

            WeaponManager weaponManager = FindObjectOfType<WeaponManager>();

            if (weaponManager == null)
            {
                Debug.LogError("WeaponManager not found in the scene!"); // Error log if WeaponManager is not found
            }
            else
            {
                // Use the instance of WeaponManager to call the method
                weaponManager.UnlockWeaponFromBlueprint(blueprint);
            }

            Destroy(gameObject);  // Remove the blueprint object from the scene

        }
    }
}
