using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] public AudioClip FirstClip;
    private AudioSource audioSource;
 
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
            audioSource.volume = 0.65f;
            //audioSource.volume = 0f;
            audioSource.Play();
            Instance = this;
        }


        DontDestroyOnLoad(this.gameObject);
    }
}