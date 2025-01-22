using UnityEngine;

public class AMLoader : MonoBehaviour
{
    public AudioManager audioManager;

    private void Awake()
    {
        if (FindAnyObjectByType<AudioManager>() == null)
        {
            AudioManager.instance = Instantiate(audioManager);
            DontDestroyOnLoad(AudioManager.instance.gameObject);
        }
    }
}
