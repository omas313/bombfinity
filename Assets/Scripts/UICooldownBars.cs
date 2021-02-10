using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownBars : MonoBehaviour
{
    [SerializeField] Image _shieldFillImage;
    [SerializeField] Shield _shield;
    [SerializeField] Animation _shieldAnimation;
    [SerializeField] AudioClip _shieldReadySound;

    AudioSource _audioSource;
    float _previousAmount;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();    
    }

    void Update()
    {
        _previousAmount = _shieldFillImage.fillAmount;
        _shieldFillImage.fillAmount = _shield.CooldownTimer / _shield.Cooldown;
        
        if (_shieldFillImage.fillAmount == 1 && _previousAmount != 1)
        {
            _audioSource.PlayOneShot(_shieldReadySound);
            _shieldAnimation.Play();
        }
    }   
}
