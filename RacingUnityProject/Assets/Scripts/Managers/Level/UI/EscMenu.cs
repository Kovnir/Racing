using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Zenject;

public class EscMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [InjectOptional] private LoaderViewManager loaderViewManager;
    
    [UsedImplicitly]
    public void OnResumeClick()
    {
        SetPause(false);
    }
    
    [UsedImplicitly]
    public void OnRestartClick()
    {
        Time.timeScale = 1;
        loaderViewManager.ReloadLevel();        
    }
    
    [UsedImplicitly]
    public void OnMainMenuClick()
    {
        Time.timeScale = 1;
        loaderViewManager.LoadMainMenu();
    }
    
    public void OnPostProcessingClick()
    {
        
    }
    
    public void OnQuitClick()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private bool pauseMode;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMode = !pauseMode;
            SetPause(pauseMode);
        }
    }

    private void SetPause(bool isPause)
    {
        Time.timeScale = isPause ? 0 :1;
        panel.SetActive(isPause);
    }
}
