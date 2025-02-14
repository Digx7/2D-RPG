using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Change

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField] StringChannel changeSceneChannel;
    [SerializeField] StringChannel addSceneChannel;
    [SerializeField] StringChannel removeSceneChannel;
    
    // CHANELS =================================

    private void OnEnable()
    {
        SetupChannels();
    }

    private void OnDisable()
    {
        TearDownChannels();
    }

    private void SetupChannels()
    {
        changeSceneChannel.channelEvent.AddListener(OnChangeScene);
        addSceneChannel.channelEvent.AddListener(OnAddScene);
        removeSceneChannel.channelEvent.AddListener(UnloadScene);

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnAcitveSceneChanged;
    }

    private void TearDownChannels()
    {
        changeSceneChannel.channelEvent.RemoveListener(OnChangeScene);
        addSceneChannel.channelEvent.RemoveListener(OnAddScene);
        removeSceneChannel.channelEvent.RemoveListener(UnloadScene);

        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnAcitveSceneChanged;
    }

    // CHANNEL RESPONES =================================

    private void OnChangeScene(string name)
    {
        LoadScene(name);
    }

    private void OnAddScene(string name)
    {
        LoadSceneMode mode = LoadSceneMode.Additive;
        LoadScene(name, mode);
    }

    private void OnAcitveSceneChanged(Scene current, Scene next)
    {
        // Exists if we need to add any code here
    }

    // MAIN FUNCTIONS =================================

    private void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name, mode);
    }

    private void UnloadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(name);
    }
}
