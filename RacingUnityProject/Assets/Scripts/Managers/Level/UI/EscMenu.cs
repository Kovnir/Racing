using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Zenject;

public class EscMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [InjectOptional] private LoaderViewManager loaderViewManager;
    public void OnResumeClick()
    {
        SetPause(false);
    }
    
    public void OnRestartClick()
    {
        Time.timeScale = 1;
        loaderViewManager.ReloadLevel();        
    }
    
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
        Application.Quit();
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
