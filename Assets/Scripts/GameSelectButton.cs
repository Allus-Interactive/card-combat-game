using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelectButton : MonoBehaviour
{
    public string levelToLoad;

    public void SelectGame()
    {
        SceneManager.LoadScene(levelToLoad);

        AudioManager.instance.PlaySFX(0);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");

        AudioManager.instance.PlaySFX(0);
    }
}
