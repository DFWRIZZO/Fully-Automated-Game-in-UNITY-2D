using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
    public void PlayGame() {
        Debug.Log("Play button pressed.");
        SceneManager.LoadScene(1);  
    }

    public void ExitGame() {
        Debug.Log("Exit button pressed.");
        Application.Quit();  
    }
}