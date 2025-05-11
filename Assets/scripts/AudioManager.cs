using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource; //for the sound effects
    public AudioClip loseSound;
    public AudioClip rainSound;
    public AudioSource musicSource;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    //for any SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    //for the lose sound
    public void PlayLoseSound()
    {
        PlaySFX(loseSound);
    }
    //for the rain challange
    public void PlayRain()
    {
        PlaySFX(rainSound);
    }
    //to stop the music
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
}
