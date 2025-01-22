using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string battleSelectScene;

    public void StartGame()
    {
        SceneManager.LoadScene(battleSelectScene);
    } 

    public void QuitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
