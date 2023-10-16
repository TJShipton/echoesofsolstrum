
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Solfather : MonoBehaviour
{

    public GameObject player;
    public float interactionDistance = 3.0f;
    public GameObject playerUpgradePanel;
    public TextMeshProUGUI promptText;
    public LayerMask clickableLayer;
    public EventSystem eventSystem;
    public CanvasGroup playerUpgradeCanvasGroup;  // Reference to the CanvasGroup component

    private bool isPlayerNear = false;
    private Camera mainCamera;
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
                             (Input.GetButtonDown("Fire2") && GameManager.instance.LastInputMethod == "controller")))
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
            promptText.text = "Press B Button";
        }
        else
        {
            promptText.text = "Press E Key";
        }
    }


    public void Interact()
    {
        TogglePlayerUpgradePanel();
    }

    public void TogglePlayerUpgradePanel()
    {
       
        // Get the CanvasGroup component from the playerUpgradePanel
        CanvasGroup playerUpgradeCanvasGroup = playerUpgradePanel.GetComponent<CanvasGroup>();

        if (playerUpgradeCanvasGroup != null)
        {
            bool isPanelActive = playerUpgradeCanvasGroup.interactable;

            // Toggle the CanvasGroup properties
            playerUpgradeCanvasGroup.interactable = !isPanelActive;
            playerUpgradeCanvasGroup.blocksRaycasts = !isPanelActive;
            playerUpgradeCanvasGroup.alpha = !isPanelActive ? 1 : 0;

            // If the panel is now active, set the first button as the selected object in the EventSystem
            if (!isPanelActive)  // since isPanelActive holds the old state, !isPanelActive is the new state
            {
                // Find the first button in the playerUpgradePanel hierarchy if it's null
                if (firstButton == null)
                {
                    firstButton = playerUpgradePanel.GetComponentInChildren<Button>();
                }

                // Ensure the firstButton is not null before trying to access it
                if (firstButton != null)
                {
                    eventSystem.SetSelectedGameObject(firstButton.gameObject);
                }
                else
                {
                    Debug.LogWarning("No button found in playerUpgradePanel hierarchy.");  // Debug log for no button found
                }
            }
        }
        else
        {
            Debug.LogError("CanvasGroup not found on playerUpgradePanel!");
        }
    }


}



