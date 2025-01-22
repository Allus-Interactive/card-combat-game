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

    private int currentBGM;
    private bool playingBGM;

    void Start()
    {
        
    }

    void Update()
    {
        if (playingBGM)
        {
            if (bgm[currentBGM].isPlaying == false)
            {
                currentBGM++;

                if (currentBGM >= bgm.Length)
                {
                    currentBGM = 0;
                }

                bgm[currentBGM].Play();
            }
        }
    }

    public void StopMusic()
    {
        mainMenu.Stop();
        gameSelection.Stop();
        foreach (AudioSource track in bgm)
        {
            track.Stop();
        }

        playingBGM = false;
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

        currentBGM = Random.Range(0, bgm.Length);

        bgm[currentBGM].Play();
        playingBGM = true;
    }
}
