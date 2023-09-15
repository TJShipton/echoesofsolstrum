using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab; // Prefab of the weapon to be picked up

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // Check if the collider is tagged as 'Player'
        {
            PickUp(other.gameObject); // Call the PickUp function
        }
    }

    void PickUp(GameObject player)
    {
        Debug.Log("PickUp called");

        WeaponManager weaponManager = player.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            Debug.Log("WeaponManager found");

            Weapon newWeapon = Instantiate(weaponPrefab, weaponManager.weaponHolder);
            newWeapon.gameObject.SetActive(false);

            if (weaponManager.weaponHolder != null)
            {
              
                newWeapon.transform.SetParent(weaponManager.weaponHolder);
                Debug.Log("WeaponHolder world position: " + weaponManager.weaponHolder.position);
                Debug.Log("Weapon world position: " + newWeapon.transform.position);
                Debug.Log("Weapon local position: " + newWeapon.transform.localPosition);


                newWeapon.transform.localPosition = Vector3.zero;
                newWeapon.transform.localRotation = Quaternion.identity;
            }
            weaponManager.availableWeapons.Add(newWeapon);
            weaponManager.SwitchWeapon(newWeapon);
        }
        Destroy(gameObject);
    }


}