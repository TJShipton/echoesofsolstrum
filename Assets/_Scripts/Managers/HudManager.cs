using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HudManager : MonoBehaviour
{
    public static HudManager instance;  // Singleton instance

    // Reference to the TextMesh Pro component for Sol display in HUD
    public TextMeshProUGUI solTextHUD;

    // Reference to the TextMesh Pro component for Sol display in Menu
    public TextMeshProUGUI solTextMenu;

    // Unity event that is triggered when Sol changes
    public UnityEvent OnSolChanged;

    [SerializeField]
    private TextMeshProUGUI weaponUnlockText; //weapon unlock text 

    [SerializeField]
    private TextMeshProUGUI modchipUnlockText; //weapon unlock text 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize UnityEvent
        if (OnSolChanged == null)
        {
            OnSolChanged = new UnityEvent();
        }

        // Add UpdateSolDisplay as a listener to the OnSolChanged event
        OnSolChanged.AddListener(UpdateSolDisplay);
    }

    void Start()
    {
        if (weaponUnlockText == null)
        {
            Debug.LogError("WeaponUnlockText is not assigned in HudManager.");
        }
        if (modchipUnlockText == null)
        {
            Debug.LogError("ModchipUnlockText is not assigned in HudManager.");
        }

        // Only find and set the solTextHUD if it's not already set
        if (solTextHUD == null)
        {
            GameObject hudObject = GameObject.Find("SolDisplayHUD");
            if (hudObject != null)
            {
                solTextHUD = hudObject.GetComponent<TextMeshProUGUI>();
            }
        }

        // Subscribe to the OnSolfatherSpawned event to update solTextMenu
        TestSolfatherSpawn.OnSolfatherSpawned += UpdateSolTextMenuReference;

        UpdateSolDisplay();
    }

    // This function will update the solTextMenu reference
    private void UpdateSolTextMenuReference(GameObject solfather)
    {
        Transform parentTransform = solfather.transform.Find("SolfatherCanvas");
        Transform panelTransform = parentTransform ? parentTransform.Find("PlayerUpgradePanel") : null;
        if (panelTransform != null)
        {
            GameObject menuObject = panelTransform.Find("TotalSol").gameObject;
            if (menuObject != null)
            {
                solTextMenu = menuObject.GetComponent<TextMeshProUGUI>();
                //Debug.Log("TotalSol reference updated");
            }
        }
    }

    // Function to update the HUD display of Sol
    public void UpdateSolDisplay()
    {
        // Update Sol display in HUD
        if (solTextHUD != null)
        {
            solTextHUD.text = " " + CurrencyManager.instance.solCurrency;
        }

        // Update Sol display in Menu (if needed)
        if (solTextMenu != null)
        {
            solTextMenu.text = " " + CurrencyManager.instance.solCurrency;
        }
    }

    public TextMeshProUGUI GetWeaponUnlockText()
    {
        return weaponUnlockText;
    }

    public TextMeshProUGUI GetModchipUnlockText()
    {
        return modchipUnlockText;
    }

}
