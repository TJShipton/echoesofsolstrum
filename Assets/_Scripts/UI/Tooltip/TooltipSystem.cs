using TMPro;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem Instance;

    public GameObject modchipTooltipPanel;
    public TextMeshProUGUI tooltipText;



    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying duplicate instance of TooltipSystem");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        tooltipText = modchipTooltipPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (tooltipText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found in ModchipTooltipPanel's children.");
            return;
        }

        HideTooltip(); // Start with the tooltip hidden
    }


    public void ShowTooltip(string text, Vector3 position)
    {
        if (modchipTooltipPanel == null || tooltipText == null)
        {
            Debug.LogError("Tooltip components are not assigned.");
            return;
        }
        modchipTooltipPanel.SetActive(true);
        tooltipText.text = text;
        UpdatePosition(position); // Pass position here
    }


    public void HideTooltip()
    {
        if (modchipTooltipPanel != null)
        {
            modchipTooltipPanel.SetActive(false);
        }
    }

    public void UpdatePosition(Vector3 position)
    {
        float offsetX = 150f; // Example offset, adjust as needed
        float offsetY = 10f; // Example offset, adjust as needed

        Vector3 tooltipPosition = position + new Vector3(offsetX, offsetY, 0);
        modchipTooltipPanel.transform.position = tooltipPosition;
    }



}
