using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelectButton : MonoBehaviour
{
    public string levelToLoad;

    public void SelectGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
