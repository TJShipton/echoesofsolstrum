using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private string tooltipText; // The text to display in the tooltip.
    private bool isGamepadActive; // To track if the gamepad is being used

    // Method to set the tooltip text from outside
    public void SetTooltipText(string text)
    {
        tooltipText = text;
    }

    // This method will be called when the object becomes the selected UI element
    public void OnSelect(BaseEventData eventData)
    {
        if (!string.IsNullOrEmpty(tooltipText))
        {
            isGamepadActive = true; // Gamepad is active
            ShowTooltip();
        }
    }

    // This method will be called when the object is no longer the selected UI element
    public void OnDeselect(BaseEventData eventData)
    {
        TooltipSystem.Instance.HideTooltip();
    }

    // This method will be called when the mouse pointer enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(tooltipText))
        {
            isGamepadActive = false; // Mouse is active
            ShowTooltip();
        }
    }

    // This method will be called when the mouse pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Instance.HideTooltip();
    }

    private void ShowTooltip()
    {
        Vector3 tooltipPosition = isGamepadActive ? GetGamepadTooltipPosition() : GetMouseTooltipPosition();
        TooltipSystem.Instance.ShowTooltip(tooltipText, tooltipPosition);
    }

    private Vector3 GetGamepadTooltipPosition()
    {
        // Convert the local position of the UI element to a screen position for gamepad
        return RectTransformUtility.WorldToScreenPoint(null, transform.position);
    }

    private Vector3 GetMouseTooltipPosition()
    {
        float offsetX = 150f;
        float offsetY = 100f;
        Vector3 mousePosition = Input.mousePosition;
        return new Vector3(mousePosition.x + offsetX, mousePosition.y + offsetY, mousePosition.z);
    }
}

