using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] public AudioClip FirstClip; // Primer audio que se reproduce una vez
    [SerializeField] public AudioClip SecondClip; // Segundo audio que se loopea

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = FirstClip;
        audioSource.loop = false; // El primer clip no debe repetirse
        audioSource.volume = 0.75f;
        audioSource.Play();
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            return;
        }

        audioSource.clip = SecondClip;
        audioSource.loop = true; // Este clip se debe repetir
        audioSource.Play();
    }
}