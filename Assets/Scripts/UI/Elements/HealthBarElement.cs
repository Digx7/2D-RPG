using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class HealthBarElement : UIElement
{
    public Health health;
    public Slider slider;
    private float maxHealth;
    private float currentHealth;
    public bool setUpOnEnable = true;

    public void Setup()
    {
        if(health != null)
        {
            health.OnDamage.AddListener(Damage);
            health.OnHeal.AddListener(Heal);

            maxHealth = health.MaxHealth;
            currentHealth = health.CurrentHealth;
        }
    }
    
    public void OnEnable()
    {
        if(setUpOnEnable)
        {
            Setup();
        }
    }

    public void OnDisable()
    {
        if(health != null)
        {
            health.OnDamage.RemoveListener(Damage);
            health.OnHeal.RemoveListener(Heal);
        }
    }

    private void Start()
    {
        if(setUpOnEnable)
        {
            maxHealth = health.MaxHealth;
            currentHealth = health.CurrentHealth;
        }
    }

    private void Render()
    {
        float value = (currentHealth / maxHealth);
        slider.value = value;
    }

    public void Damage(DamageResult damageResult)
    {
        currentHealth -= damageResult.trueDamage.amount;
        if(currentHealth < 0) currentHealth = 0;
        Render();
    }

    public void Heal(DamageResult damageResult)
    {
        currentHealth += damageResult.trueDamage.amount;
        if(currentHealth > maxHealth) currentHealth = maxHealth;
        Render();
    }

    
}

