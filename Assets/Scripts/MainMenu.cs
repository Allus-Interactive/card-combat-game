using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string battleSelectScene;

    void Start()
    {
        AudioManager.instance.PlayMenuMusic();    
    }

    public void StartGame()
    {
        SceneManager.LoadScene(battleSelectScene);

        AudioManager.instance.PlaySFX(0);
    } 

    public void QuitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();

        AudioManager.instance.PlaySFX(0);
    }
}
