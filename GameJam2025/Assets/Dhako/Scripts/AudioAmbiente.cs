using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAmbiente : MonoBehaviour
{
    private AudioSource _audioSource;
    public float _contador = 0;
    private Animator _animator;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<Animator>();
        NextTimeToCount();
    }

    // Update is called once per frame
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
        _contador= Random.Range(5, 20);
    }
}
