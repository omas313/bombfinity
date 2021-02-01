using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] Image _fillImage;
    [SerializeField] GameObject _target;


    void Start()
    {
        if (_target != null)
            _target.GetComponent<IHealth>().HealthChanged += OnHealthChanged;
        else 
            GetComponentInParent<IHealth>().HealthChanged += OnHealthChanged;
    }

    void OnHealthChanged(int currentHealth, int maxHealth)
    {
        _fillImage.fillAmount = (float)currentHealth / (float)maxHealth;
    }

}
