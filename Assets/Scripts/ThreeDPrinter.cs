using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThreeDPrinter : MonoBehaviour
{
    public GameObject uiPanel;
    public Button[] weaponButtons;
    public Text debugText;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            return;
        }

        if (other.tag == "Player")
        {
            ShowWeaponOptions();
        }
    }

    private void ShowWeaponOptions()
    {
        if (GameManager.instance == null || weaponButtons == null)
        {
            return;
        }

        uiPanel.SetActive(true);

        foreach (Button btn in weaponButtons)
        {
            btn.onClick.RemoveAllListeners();
        }

        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (i < GameManager.instance.unlockedWeapons.Count)  // Notice it's Count, not Length, for lists
            {
                TextMeshProUGUI buttonText = weaponButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText == null)
                {
                    return;
                }
                buttonText.text = GameManager.instance.unlockedWeapons[i].name;
                weaponButtons[i].gameObject.SetActive(true);

                string localWeaponName = GameManager.instance.unlockedWeapons[i].name;
                AssignListener(weaponButtons[i], localWeaponName);
            }
            else
            {
                weaponButtons[i].gameObject.SetActive(false);
            }
        }


    }

    private void AssignListener(Button button, string weaponName)
    {
        button.onClick.AddListener(() => PickWeapon(weaponName));
    }

    public void PickWeapon(string weaponName)
    {
        Debug.Log("Weapon Name to Pick: " + weaponName);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            WeaponManager weaponManager = player.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                GameObject weaponPrefab = GameManager.instance.GetWeaponPrefabByName(weaponName);
                if (weaponPrefab != null)
                {
                    Debug.Log("Weapon Prefab to Instantiate: " + weaponPrefab.name);
                    Weapon newWeapon = Instantiate(weaponPrefab.GetComponent<Weapon>(), weaponManager.weaponHolder);
                    newWeapon.gameObject.SetActive(false);
                    newWeapon.transform.SetParent(weaponManager.weaponHolder);

                    newWeapon.transform.localPosition = Vector3.zero;
                    newWeapon.transform.localRotation = Quaternion.identity;

                    weaponManager.availableWeapons.Add(newWeapon);
                    weaponManager.SwitchWeapon(newWeapon);
                }
            }
        }

        uiPanel.SetActive(false);
    }
}
