using UnityEngine;

public class GameSelection : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayGameSelectionMusic();
    }
}
