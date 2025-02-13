using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAmbiente : MonoBehaviour
{
    private AudioSource _audioSource;
    private Animator _animator;
    public float _contador = 0;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<Animator>();
        NextTimeToCount();
    }
    void Update()
    {
        if (_contador <= 0)
        {
            _animator.SetTrigger("Start");
            _audioSource.Play();
            NextTimeToCount();
        }
        else _contador -= Time.deltaTime * 1;
    }

    private void NextTimeToCount()
    {
        _contador = Random.Range(5, 20);
    }
}
