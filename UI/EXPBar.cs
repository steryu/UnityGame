using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    [SerializeField] private Image _EXPbarImage;
    private float maxEXP = 100f;

    private void Start()
    {
        _EXPbarImage.fillAmount = 0;
    }

    private void Levelup()
    {
        _EXPbarImage.fillAmount = 0;
        maxEXP *= 1.2f;
    }
    public void UpdateEXPBar(float newEXP)
    {
        _EXPbarImage.fillAmount = newEXP / maxEXP;
    }
    public bool CheckLevelUp(float EXP)
    {
        if (EXP >= maxEXP)
        {
            Levelup();
            return true;
        }
        return false;
    }
}
