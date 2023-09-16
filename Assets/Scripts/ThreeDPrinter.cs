// 3DPrinter.cs renamed to ThreeDPrinter
using UnityEngine;
using UnityEngine.UI;

public class ThreeDPrinter : MonoBehaviour
{
    public GameObject uiPanel; // UI Panel that holds the buttons for weapon selection
    public Button[] weaponButtons; // An array of Buttons that will be used to display the weapon options

    // Triggered when something collides with this GameObject
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called");
        if (other.tag == "Player") // Check if the collider is tagged as 'Player'
        {
            ShowWeaponOptions(); // Show the weapon selection UI
        }
    }

    // Method to activate the weapon selection UI and populate it based on the unlocked weapons
    private void ShowWeaponOptions()
    {
        uiPanel.SetActive(true); // Make the UI panel visible

        // Clear existing onClick listeners to avoid stacking
        foreach (Button btn in weaponButtons)
        {
            btn.onClick.RemoveAllListeners();
        }

        // Iterate through each button in the weaponButtons array
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            // Check if the index 'i' is within the bounds of the UnlockedWeapons list
            if (i < GameManager.instance.UnlockedWeapons.Count)
            {
                Debug.Log("Activating button: " + i);
                weaponButtons[i].gameObject.SetActive(true); // Activate this button
                                                             // Change the button text to the name of the weapon in the UnlockedWeapons list
                weaponButtons[i].GetComponentInChildren<Text>().text = GameManager.instance.UnlockedWeapons[i];

                string localWeaponName = GameManager.instance.UnlockedWeapons[i]; // use a local variable

                // Add a click listener to this button that will call the PickWeapon method when clicked
                weaponButtons[i].onClick.AddListener(() => PickWeapon(localWeaponName));
            }
            else
            {
                Debug.Log("Deactivating button: " + i);
                weaponButtons[i].gameObject.SetActive(false); // Deactivate this button as there's no weapon for it
            }
        }
    }


    // Method that's called when a weapon button is clicked
    public void PickWeapon(string weaponName)
    {
        Debug.Log("PickWeapon called with: " + weaponName);
        // Get a random weapon from the GameManager's pool of unlocked weapons
        GameObject weaponPrefab = GameManager.instance.GetRandomWeaponFromPool();
        if (weaponPrefab != null)
        {
            // Implement logic here to equip this weapon to the player
            // This can be similar to the PickUp method you have in your WeaponPickup class
        }

        uiPanel.SetActive(false); // Hide the weapon selection UI
    }
}
