using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Image _fillBar;
    [SerializeField] GameObject _target;


    void Awake()
    {
        _target.GetComponent<IHealth>().HealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int current, int max)
    {
        _fillBar.fillAmount = (float) current / (float) max;
    }
}
