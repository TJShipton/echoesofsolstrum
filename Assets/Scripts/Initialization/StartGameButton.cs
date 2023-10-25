using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("HomeScene");  // Replace with your actual game scene name
    }
}
