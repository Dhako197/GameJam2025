using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] public AudioClip FirstClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = FirstClip;
        audioSource.loop = true;
        //audioSource.volume = 0.65f;
        audioSource.volume = 0f;
        audioSource.Play();
    }
}