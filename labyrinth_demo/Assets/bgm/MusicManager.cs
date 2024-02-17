using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Static instance of the MusicManager
    private static MusicManager instance;

    // Reference to the AudioClip
    public AudioClip audioClip;
    // Reference to the AudioSource component
    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure there's only one instance of MusicManager
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        PlayMusic();
    }

    // Play the music
    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    // Stop the music
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}