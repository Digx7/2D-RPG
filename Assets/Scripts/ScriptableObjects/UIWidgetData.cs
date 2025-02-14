using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewUIWidgetData", menuName = "ScriptableObjects/UI/WidgetData", order = 1)]
public class UIWidgetData : ScriptableObject
{
    public GameObject widgetPrefab;

    private GameObject instantiatedWidget;

    public GameObject GetInstantiatedWidget()
    {
        return instantiatedWidget;
    }

    public void SpawnWidget(Transform parent = null)
    {
        if(parent != null)
        {
            instantiatedWidget = Instantiate(widgetPrefab, parent);
        }

        UIWidget uIWidget = instantiatedWidget.GetComponent<UIWidget>();
        uIWidget.Setup(this);
    }

    public void DespawnWidget()
    {
        UIWidget uIWidget = instantiatedWidget.GetComponent<UIWidget>();
        uIWidget.Teardown();
    }
}