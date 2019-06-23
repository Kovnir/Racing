using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using Managers.Camera;
using TMPro;
using UnityEditor;
using UnityEngine;
using Zenject;

public class EscMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI postProcessingText;
    [SerializeField] private TextMeshProUGUI cameraModeText;
    [InjectOptional] private LoaderViewManager loaderViewManager;
    [Inject] private PlayerProfileManager playerProfileManager;
    
    
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
    
    [UsedImplicitly]
    public void OnPostProcessingClick()
    {
        playerProfileManager.ChangePostProcessingState();
        UpdatePostProcessingButtonText();
    }
    
    [UsedImplicitly]
    public void OnCameraModeClick()
    {
        playerProfileManager.ChangeCameraModeState();
        UpdateCameraModeButtonText();
    }
    
    [UsedImplicitly]
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
        if (isPause)
        {
            UpdatePostProcessingButtonText();
            UpdateCameraModeButtonText();
        }
    }

    private void UpdatePostProcessingButtonText()
    {
        bool isPostProcessingEnabled = playerProfileManager.GetPostProcessingState();
        postProcessingText.text = (isPostProcessingEnabled ? "Disable" : "Enable") + " Post Processing";
    }
    
    private void UpdateCameraModeButtonText()
    {
        CameraMode cameraMode = playerProfileManager.GetCameraModeState();
        cameraModeText.text = "Camera Mode (" + cameraMode + ")";
    }
}
