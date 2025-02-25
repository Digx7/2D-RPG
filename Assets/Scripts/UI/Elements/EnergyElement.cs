using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class EnergyElement : UIElement
{
    public TextMeshProUGUI countTextMeshPro;
    public int startingEnergy;
    private int currentEnergy;

    public void OnEnable()
    {
        
    }

    public void OnDisable()
    {
        
    }

    private void Start()
    {
        currentEnergy = startingEnergy;
    }

    private void Render()
    {
        countTextMeshPro.text = "" + currentEnergy;
    }

    public void SetEnergy(int energy)
    {
        currentEnergy = energy;
        Render();
    }

    

    
}

