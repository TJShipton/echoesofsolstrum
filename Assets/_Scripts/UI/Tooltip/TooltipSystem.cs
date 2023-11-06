using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem Instance;

    public GameObject tooltipPanel;
    public Text tooltipText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep the instance alive across scenes.

        HideTooltip(); // Start with the tooltip hidden
    }


    public void ShowTooltip(string text)
    {
        if (tooltipPanel == null || tooltipText == null)
        {
            Debug.LogError("Tooltip components are not assigned.");
            return;
        }
        tooltipPanel.SetActive(true);
        tooltipText.text = text;
        UpdatePosition();
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    private void UpdatePosition()
    {
        // Ensure that the tooltip follows the mouse or is positioned correctly.
        Vector2 position = Input.mousePosition;
        // Adjust the position here if needed
        tooltipPanel.transform.position = position;
    }

    private void Update()
    {
        // If the tooltip is active, make sure it follows the cursor or updates its position.
        if (tooltipPanel.activeSelf)
        {
            UpdatePosition();
        }
    }
}
