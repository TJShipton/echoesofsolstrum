using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private IInteractable currentOpenMenu;
    public bool IsAnyMenuOpen { get; private set; }

    public delegate void PlayerStateHandler(bool isMenuOpen);
    public static event PlayerStateHandler OnMenuStatusChange;



    void Update()
    {
        if (UIManager.Instance.IsAnyMenuOpen)
        {
            // UI is active, so don't process other inputs or gameplay updates
            return;
        }


    }



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

        }
        else
        {
            Destroy(gameObject);
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

        // Notify any listeners that the menu status has changed
        OnMenuStatusChange?.Invoke(IsAnyMenuOpen);
    }

    public void CloseCurrentMenu()
    {
        if (currentOpenMenu != null)
        {
            currentOpenMenu.CloseMenu();
            currentOpenMenu = null;
            IsAnyMenuOpen = false;

            // Notify any listeners that the menu status has changed
            OnMenuStatusChange?.Invoke(IsAnyMenuOpen);
        }
    }
}
