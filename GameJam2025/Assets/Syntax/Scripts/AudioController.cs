using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] private AudioClip FirstClip;
    private AudioSource audioSource;
    private readonly float baseVolume = 0.65f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = FirstClip;
            audioSource.loop = true;
            audioSource.volume = baseVolume;
            //audioSource.volume = 0f;
            audioSource.Play();
            Instance = this;
        }


        DontDestroyOnLoad(this.gameObject);
    }

    public void SetVolume()
    {
        audioSource.volume = baseVolume;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}