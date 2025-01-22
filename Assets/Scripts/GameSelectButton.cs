using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelectButton : MonoBehaviour
{
    public string levelToLoad;

    public void SelectGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
