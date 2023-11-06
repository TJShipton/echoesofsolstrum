using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private IInteractable currentOpenMenu;
    public bool IsAnyMenuOpen { get; private set; }



    void Update()
    {
        if (UIManager.Instance.IsAnyMenuOpen)
        {
            // UI is active, so don't process other inputs or gameplay updates
            return;
        }

        // Regular input and gameplay update code...
    }



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void HandleMenuOpen(IInteractable interactable)
    {
        if (currentOpenMenu != null)
        {
            currentOpenMenu.CloseMenu();
        }

        interactable.OpenMenu();
        currentOpenMenu = interactable;
        IsAnyMenuOpen = true;
    }

    public void CloseCurrentMenu()
    {
        if (currentOpenMenu != null)
        {
            currentOpenMenu.CloseMenu();
            currentOpenMenu = null;
            IsAnyMenuOpen = false;
        }
    }
}
