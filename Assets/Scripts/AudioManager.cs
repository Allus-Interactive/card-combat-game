using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public AudioSource mainMenu;
    public AudioSource gameSelection;
    public AudioSource[] bgm;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StopMusic()
    {
        mainMenu.Stop();
        gameSelection.Stop();
        foreach (AudioSource track in bgm)
        {
            track.Stop();
        }
    }

    public void PlayMenuMusic()
    {
        StopMusic();
        mainMenu.Play();
    }

    public void PlayGameSelectionMusic()
    {
        StopMusic();
        gameSelection.Play();
    }

    public void PlayBGM()
    {
        StopMusic();
    }
}
