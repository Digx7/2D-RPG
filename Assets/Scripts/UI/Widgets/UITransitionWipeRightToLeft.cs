using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UITransitionWipeRightToLeft : UIWidget
{

    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_lifeTime;
    
    public override void Setup(UIWidgetData newUIWidgetData)
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        base.Setup(newUIWidgetData);
    }

    public override void Teardown()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChanged;
        base.Teardown();
    }

    public void OnSceneChanged(Scene current, Scene next)
    {
        m_animator.SetTrigger("SceneHasLoaded");
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(m_lifeTime);

        UnloadSelf();
    }
}
