using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThreeDPrinter : MonoBehaviour
{
    public GameObject uiPanel;
    public Button weaponButtonPrefab;
    public Dictionary<string, Sprite> weaponThumbnails;
   
    public float buttonOffsetX = 0.0f;

    private void Start()
    {
        weaponThumbnails = new Dictionary<string, Sprite>();

        Sprite conductiveGloveThumbnail = Resources.Load<Sprite>("WeaponThumbnails/ConductiveGlove");
        weaponThumbnails.Add("Conductive Glove", conductiveGloveThumbnail);

        Sprite glockNSteelThumbnail = Resources.Load<Sprite>("WeaponThumbnails/GlockNSteel");
        weaponThumbnails.Add("Glock 'n' Steel", glockNSteelThumbnail);

        Sprite hyperbassFluteThumbnail = Resources.Load<Sprite>("WeaponThumbnails/HyperbassFlute");
        weaponThumbnails.Add("Hyperbass Flute", hyperbassFluteThumbnail);

        Sprite drumstickThumbnail = Resources.Load<Sprite> ("WeaponThumbnails/Drumstick");
        weaponThumbnails.Add("Drumstick", drumstickThumbnail);

        // Add more thumbnails here...
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            return;
        }

        if (other.tag == "Player")
        {
            // Enable the UI panel (which is now a child of this GameObject)
            uiPanel.SetActive(true);
            ShowWeaponOptions();
        }
    }

    private void ShowWeaponOptions()
    {
        if (GameManager.instance == null)
        {
            return; // Early return if GameManager instance is not available
        }

        // Clear previous buttons
        foreach (Transform child in uiPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Get random 3 weapons from the list of unlocked weapons
        List<GameObject> randomWeapons = GameManager.instance.GetRandomUnlockedWeapons(3);

        // Loop through each randomly selected weapon
        foreach (GameObject weapon in randomWeapons)
        {
            // Instantiate the button and parent it to the UI panel
            Button newButton = Instantiate(weaponButtonPrefab, uiPanel.transform);

            // Set the button text and image
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = weapon.name;

            Image buttonImage = newButton.GetComponentInChildren<Image>();
            if (weaponThumbnails.ContainsKey(weapon.name))
            {
                buttonImage.sprite = weaponThumbnails[weapon.name];
            }

            // Assign the button's click listener
            newButton.onClick.AddListener(() => PickWeapon(weapon.name));

            // Debug log to confirm which weapons are being displayed
            Debug.Log("Displaying weapon: " + weapon.name);
        }
    }


    private void AssignListener(Button button, string weaponName)
    {
        button.onClick.AddListener(() => PickWeapon(weaponName));
    }

    public void PickWeapon(string weaponName)
    {
        if (string.IsNullOrEmpty(weaponName))
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            WeaponManager weaponManager = player.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                GameObject weaponPrefab = GameManager.instance.GetWeaponPrefabByName(weaponName);
                if (weaponPrefab != null)
                {
                    Weapon newWeapon = Instantiate(weaponPrefab.GetComponent<Weapon>(), weaponManager.weaponHolder);
                    newWeapon.gameObject.SetActive(false);
                    newWeapon.transform.SetParent(weaponManager.weaponHolder);

                    newWeapon.transform.localPosition = newWeapon.localPosition;
                    newWeapon.transform.localRotation = Quaternion.identity;
                    newWeapon.transform.localEulerAngles = newWeapon.localOrientation;

                    weaponManager.availableWeapons.Add(newWeapon);
                    weaponManager.SwitchWeapon(newWeapon);
                }
            }
        }

        uiPanel.SetActive(false);
    }
}
