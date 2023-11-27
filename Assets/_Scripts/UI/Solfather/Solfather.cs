
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Solfather : MonoBehaviour, IInteractable
{

    public GameObject player;
    public float interactionDistance = 3.0f;
    public InputActionAsset inputActions;

    public GameObject playerUpgradePanel;
    public TextMeshProUGUI promptText;
    public LayerMask clickableLayer;
    public EventSystem eventSystem;
    public CanvasGroup playerUpgradeCanvasGroup;  // Reference to the CanvasGroup component

    private bool isPlayerNear = false;

    private bool isPlayerUpgradePanelActive = false;
    private Button firstButton = null;



    // Start is called before the first frame update
    void Start()
    {
        // Initialize EventSystem if it's null
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();

        }
    }

    void OnEnable()
    {
        // Activate the UI action map
        var uiActionMap = inputActions.FindActionMap("UI");
        if (uiActionMap != null)
        {
            uiActionMap.Enable();
        }
        else
        {
            Debug.LogError("UI Action Map not found in Input Actions.");
        }
    }

    void OnDisable()
    {
        // Deactivate the UI action map
        var uiActionMap = inputActions.FindActionMap("UI");
        if (uiActionMap != null)
        {
            uiActionMap.Disable();
        }
    }


    // Update is called once per frame
    private void Update()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

        // Only proceed if the player is within a certain distance (e.g., 2 units)
        if (distanceToPlayer <= 2f)
        {
            isPlayerNear = true;
            UpdatePromptText();  // Update the prompt text
        }
        else
        {
            isPlayerNear = false;
            promptText.gameObject.SetActive(false);  // Hide the prompt
        }

        // If the player is near and presses the 'E' key or the 'B' button, toggle the panel
        if (isPlayerNear && ((Input.GetKeyDown(KeyCode.E) && GameManager.instance.LastInputMethod == "keyboard") ||
                             (Input.GetButtonDown("Fire3") && GameManager.instance.LastInputMethod == "controller")))
        {

            TogglePlayerUpgradePanel();
        }
    }



    private void UpdatePromptText()
    {
        if (!isPlayerNear || isPlayerUpgradePanelActive)  // Added isWeaponPanelActive to the condition
        {
            promptText.gameObject.SetActive(false);  // Hide the prompt
            return;
        }

        // Show the prompt
        promptText.gameObject.SetActive(true);

        if (GameManager.instance.LastInputMethod == "controller")
        {
            promptText.text = "Press B";
        }
        else
        {
            promptText.text = "Press E";
        }
    }


    public void Interact()
    {
        TogglePlayerUpgradePanel();
    }

    // Toggle between opening and closing the player upgrade panel
    public void TogglePlayerUpgradePanel()
    {
        // Check if the player is near, if not, return early.
        if (!isPlayerNear) return;

        if (isPlayerUpgradePanelActive)
        {
            UIManager.Instance.CloseCurrentMenu();
            UnpauseGame();  // Unpause the game when the panel is closed
        }
        else
        {
            UIManager.Instance.HandleMenuOpen(this);
            PauseGame();  // Pause the game when the panel is opened
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    // Call this method to open the player upgrade panel
    public void OpenMenu()
    {
        SetPlayerUpgradePanelActive(true);
        SetPromptTextActive(false);
    }

    // Call this method to close the player upgrade panel
    public void CloseMenu()
    {
        SetPlayerUpgradePanelActive(false);
        SetPromptTextActive(true);
    }

    // Sets the player upgrade panel active or inactive
    private void SetPlayerUpgradePanelActive(bool isActive)
    {
        if (playerUpgradeCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup not found on playerUpgradePanel!");
            return;
        }

        playerUpgradeCanvasGroup.interactable = isActive;
        playerUpgradeCanvasGroup.blocksRaycasts = isActive;
        playerUpgradeCanvasGroup.alpha = isActive ? 1 : 0;
        isPlayerUpgradePanelActive = isActive;

        if (isActive)
        {
            SelectFirstButtonInPanel();
        }
    }

    // Selects the first button in the upgrade panel
    private void SelectFirstButtonInPanel()
    {
        // Find the first button if it's null
        if (firstButton == null)
        {
            firstButton = playerUpgradePanel.GetComponentInChildren<Button>();
        }

        if (firstButton != null)
        {
            eventSystem.SetSelectedGameObject(firstButton.gameObject);
        }
        else
        {
            Debug.LogWarning("No button found in playerUpgradePanel hierarchy.");
        }
    }

    // Sets the prompt text active or inactive
    private void SetPromptTextActive(bool isActive)
    {
        promptText.gameObject.SetActive(isActive);
        if (isActive)
        {
            UpdatePromptText();
        }
    }


}



