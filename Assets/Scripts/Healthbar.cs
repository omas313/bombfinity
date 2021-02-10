using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Image _fillBar;
    [SerializeField] GameObject _target;

    Animation _animation;

    void Awake()
    {
        _target.GetComponent<IHealth>().HealthChanged += OnHealthChanged;
        _animation = GetComponent<Animation>();
    }

    void OnHealthChanged(int current, int max)
    {
        var previousFill = _fillBar.fillAmount;
        _fillBar.fillAmount = (float) current / (float) max;

        if (_fillBar.fillAmount < previousFill)
            _animation.Play();
    }
}
