using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;  // Singleton instance

    // Reference to the TextMesh Pro component
    public TextMeshProUGUI solText;

    // Awake is called when the script instance is being loaded
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
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateSolDisplay();
    }

    // Function to update the display of Sol
    public void UpdateSolDisplay()
    {
        // Display the amount of Sol from GameManager
        solText.text = "Sol: " + GameManager.instance.sol;
        Debug.Log("Sol UI updated: " + GameManager.instance.sol);  // Debug log to confirm the UI update
    }
}
