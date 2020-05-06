using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    private Enemy enemy;
    [SerializeField] private Image healthHeart;
    private float maxHealth;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        maxHealth = enemy.health;
    }

    public void updateHealth(float currentHealth)
    {
        Debug.Log(currentHealth / maxHealth + " Hoss Health");
        healthHeart.fillAmount = currentHealth / maxHealth;
    }
}
