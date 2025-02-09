using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblerSoundController : MonoBehaviour
{
    private AudioSource audioController;
    private BubblerScript bubblerScript;
    private Animator animator;
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip talkingClip;
    [SerializeField] private AudioClip spittingingClip;

    private readonly string isTalkingAnimation = "Talk";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioController = GetComponent<AudioSource>();
        bubblerScript = GameObject.FindObjectOfType<BubblerScript>();
        audioController.loop = false;
    }

    public void StopSounds()
    {
        audioController.clip = null;
        audioController.loop = false;
        audioController.Stop();
    }

    public void PlayWalking()
    {
        if (audioController.clip == walkingClip)
        {
            return;
        }
        audioController.loop = true;
        audioController.clip = walkingClip;
        audioController.Play();
    }

    public void PlayTalking()
    {
        audioController.loop = false;
        audioController.clip = talkingClip;
        audioController.Play();
        animator.SetBool(isTalkingAnimation, false);
    }

    public void PlaySpitting()
    {
        audioController.loop = false;
        audioController.clip = spittingingClip;
        audioController.Play();
    }

    public void StartShoot()
    {
        bubblerScript.ThrowBullet();
    }

    public void FinishCooldown()
    {
        bubblerScript.FinishCooldown();
    }
}
