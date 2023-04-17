using UnityEngine;

public class AudioManager : SingletonManager<AudioManager>
{
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    public AudioSource AudioSource
    {
        get { return audioSource; }
        set { audioSource = value; }
    }

    override protected void Awake()
    {
        base.Awake();
        // Create an AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}