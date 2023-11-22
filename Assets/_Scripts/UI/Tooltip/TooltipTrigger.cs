using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{

    private bool isGamepadActive; // To track if the gamepad is being used
    private TooltipSystem tooltipSystem;
    private ModchipData modchipData;


    private void Awake()
    {
        // Initialize tooltipSystem from the singleton instance
        tooltipSystem = TooltipSystem.instance;
    }


    public void SetModchipData(ModchipData data)
    {
        modchipData = data;
    }


    public void OnSelect(BaseEventData eventData)
    {
        if (modchipData != null)
        {
            isGamepadActive = true; // Gamepad is active
            ShowTooltip();
        }
    }


    public void OnDeselect(BaseEventData eventData)
    {
        TooltipSystem.instance.HideTooltip();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (modchipData != null)
        {
            isGamepadActive = false; // Mouse is active
            ShowTooltip();
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.instance.HideTooltip();
    }

    private void ShowTooltip()
    {
        Vector3 tooltipPosition = isGamepadActive ? GetGamepadTooltipPosition() : GetMouseTooltipPosition();
        TooltipSystem.instance.ShowTooltip(modchipData, tooltipPosition);

    }

    private Vector3 GetGamepadTooltipPosition()
    {
        // Convert the local position of the UI element to a screen position for gamepad
        return RectTransformUtility.WorldToScreenPoint(null, transform.position);
    }

    private Vector3 GetMouseTooltipPosition()
    {
        float offsetX = 120f;
        float offsetY = 20f;
        Vector3 mousePosition = Input.mousePosition;
        return new Vector3(mousePosition.x + offsetX, mousePosition.y + offsetY, mousePosition.z);
    }
}

