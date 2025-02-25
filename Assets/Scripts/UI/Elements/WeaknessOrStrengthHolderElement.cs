using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class WeaknessOrStrengthHolderElement : UIElement
{
    public GameObject weaknessOrStrengthElementPrefab;
    public Transform weaknessOrStrenghtElementHolder;
    public Health health;

    public void Start()
    {
        Setup();
    }

    public void Setup()
    {
        foreach (Modifier modifier in health.Modifiers)
        {
            GameObject obj = Instantiate(weaknessOrStrengthElementPrefab, weaknessOrStrenghtElementHolder);
            WeaknessOrStrengthElement weaknessOrStrengthElement = obj.GetComponent<WeaknessOrStrengthElement>();
            weaknessOrStrengthElement.Setup(modifier);
        }
    }
}

