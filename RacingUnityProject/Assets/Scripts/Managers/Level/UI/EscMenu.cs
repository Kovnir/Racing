using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    public void OnResumeClick()
    {
        SetPause(false);
    }
    
    public void OnRestartClick()
    {
        
    }
    
    public void OnMainMenuClick()
    {
        
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
