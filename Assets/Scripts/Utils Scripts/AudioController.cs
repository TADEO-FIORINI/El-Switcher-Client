using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

    public AudioClip pressKeyClip;
    public AudioClip musicClip;

    public bool sfxActive;
    public bool musicActive;

    void Start()
    {
        sfxActive = true;
        musicActive = true;
        //PlayMusic();
    }

    void PlayMusic()
    {
        if (musicActive)
        {
            musicSource.loop = true;
            musicSource.clip = musicClip;
            musicSource.Play();
        }
    }

    public void PlaySfx(AudioClip sfxClip)
    {
        if (sfxActive)
        {
            sfxSource.loop = false;
            sfxSource.clip = sfxClip;
            sfxSource.Play();
        }
    }
}