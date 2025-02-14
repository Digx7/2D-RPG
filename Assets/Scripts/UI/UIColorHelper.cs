using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIColorHelper : MonoBehaviour
{
    public List<Color> colors;
    private const float LERPTIME = 0.2f;
    private Color ogColor;
    private Color newColor;

    public void SetColorIndex(int index)
    {
        if(index >= colors.Count) return;

        ogColor = GetOGColor();
        newColor = colors[index];

        StopAllCoroutines();
        StartCoroutine(ColorFade());
    }

    IEnumerator ColorFade()
    {
        float time = 0f;
        while(time < LERPTIME)
        {
            time += Time.deltaTime;
            float t = time;

            t = t.Remap(0f, LERPTIME, 0f, 1f);
            Color updatingColor = Color.Lerp(ogColor, newColor, t);
            UpdateColor(updatingColor);
            yield return null;
        }
    }

    protected virtual Color GetOGColor()
    {
        return Color.white;
    }

    protected virtual void UpdateColor(Color newColor)
    {
        // Will be overriden by children
    }
}
