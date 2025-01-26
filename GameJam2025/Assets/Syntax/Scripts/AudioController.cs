using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip firstClip; // Primer audio que se reproduce una vez
    public AudioClip secondClip; // Segundo audio que se loopea

    private AudioSource audioSource;

    void Start()
    {
        // Obtén el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el GameObject.");
            return;
        }

        // Configura y reproduce el primer clip
        audioSource.clip = firstClip;
        audioSource.loop = false; // El primer clip no debe repetirse
        audioSource.Play();

        // Programa el segundo clip para que inicie exactamente cuando termine el primero
        double startTime = AudioSettings.dspTime + firstClip.length;
        audioSource.SetScheduledEndTime(startTime); // Opcional para precisión
        audioSource.clip = secondClip;
        audioSource.loop = true; // Este clip se debe repetir
        audioSource.PlayScheduled(startTime);
    }
}