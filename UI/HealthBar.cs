using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthbarImage;

    public void UpdateHealthBar(float maxHealth, float currenthealth)
    {
        _healthbarImage.fillAmount = currenthealth / maxHealth;
    }
}

