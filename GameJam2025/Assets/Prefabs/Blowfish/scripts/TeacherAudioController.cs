using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherAudioController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip snoreClip;
    [SerializeField] private AudioClip blopClip;
    [SerializeField] private AudioClip talkingClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    public void PlaySleeping()
    {
        audioSource.loop = true;
        audioSource.clip = snoreClip;
        audioSource.Play();
    }

    public void PlayBlop()
    {
        audioSource.loop = false;
        audioSource.clip = blopClip;
        audioSource.Play();
    }

    public void PlayTalking()
    {
        if (audioSource.clip == talkingClip)
        {
            return;
        }

        audioSource.loop = true;
        audioSource.clip = talkingClip;
        audioSource.Play();
    }
}
