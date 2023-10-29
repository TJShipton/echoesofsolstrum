using UnityEngine;
using TMPro;

public class BlueprintPickup : MonoBehaviour
{
    public WeaponBlueprint blueprint;
    [SerializeField] 
    private TextMeshProUGUI unlockText;
    public float displayDuration = 2.0f;




    private void Start()
    {
        unlockText = GameManager.instance.GetUnlockText();

        if (unlockText == null)
        {
            Debug.LogError("Unlock Text not assigned!");
        }
        else
        {
            unlockText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Immediately hide the Blueprint object upon collision
            HideBlueprintObject();

            WeaponManager weaponManager = FindObjectOfType<WeaponManager>();

            if (weaponManager == null)
            {
                Debug.LogError("WeaponManager not found in the scene!"); // Error log if WeaponManager is not found
            }
            else
            {
                // Use the instance of WeaponManager to call the method
                weaponManager.UnlockWeaponFromBlueprint(blueprint);

                // Show unlock text
                ShowUnlockText($"{blueprint.weaponName} Blueprint Unlocked");
            }
        }
    }


    private void ShowUnlockText(string message)
    {
        if (unlockText != null)
        {
            unlockText.text = message;
            unlockText.gameObject.SetActive(true);
           
            StartCoroutine(HideUnlockTextAfterDelay());
        }
        else
        {
            Debug.LogError("Unlock Text not assigned!");
        }
    }

    private System.Collections.IEnumerator HideUnlockTextAfterDelay()
    {
        
        yield return new WaitForSeconds(displayDuration);
        
        if (unlockText != null)
        {
            unlockText.gameObject.SetActive(false);
            
        }

        HideBlueprintObject(); // Call the method to hide the Blueprint object
    }


    private void HideBlueprintObject()
    {
        // Disable all the renderers in the Blueprint object and its children
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        // Disable the collider so it can't be interacted with again
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

}
