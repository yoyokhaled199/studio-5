using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    // Code review : add some UI to choose your character type
    // And find a way to save it or make it persistent 
    // so that you can retrieve the proper config when the level is loaded


    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();

    }
}