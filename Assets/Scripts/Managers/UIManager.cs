using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;  // Singleton instance

    // Reference to the TextMesh Pro component for Sol display in HUD
    public TextMeshProUGUI solTextHUD;

    // Reference to the TextMesh Pro component for Sol display in Menu
    public TextMeshProUGUI solTextMenu;

    // Unity event that is triggered when Sol changes
    public UnityEvent OnSolChanged;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
        // Only find and set the solTextHUD if it's not already set
        if (solTextHUD == null)
        {
            GameObject hudObject = GameObject.Find("SolDisplayHUD");
            if (hudObject != null)
            {
                solTextHUD = hudObject.GetComponent<TextMeshProUGUI>();
            }
        }

        // Only find and set the solTextMenu if it's not already set
        if (solTextMenu == null)
        {
            GameObject menuObject = GameObject.Find("TotalSol");
            if (menuObject != null)
            {
                solTextMenu = menuObject.GetComponent<TextMeshProUGUI>();
            }
        }

        UpdateSolDisplay();
    }


    // Function to update the display of Sol 
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

}
