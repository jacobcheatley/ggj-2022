using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;

    public void Set(Unit unit)
    {
        healthImage.fillAmount = (float)unit.currentHealth / (float)unit.maxHealth;
    }
}