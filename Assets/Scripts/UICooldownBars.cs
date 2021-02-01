using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownBars : MonoBehaviour
{
    [SerializeField] Image _shieldFillImage;
    [SerializeField] Shield _shield;


    void Update()
    {
        _shieldFillImage.fillAmount = _shield.CooldownTimer / _shield.Cooldown;
    }
}
