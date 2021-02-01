using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    float _timer;

    AudioSource _audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        _audioSource = GetComponent<AudioSource>();
        InvokeRepeating("IncreasePitch", 0f, 30f);
        DontDestroyOnLoad(gameObject);
    }

    void IncreasePitch()
    {
        _audioSource.pitch += 0.01f;
    }
}
