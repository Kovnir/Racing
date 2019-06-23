using System;
using System.Collections.Generic;
using Managers.Camera;
using Newtonsoft.Json.Linq;
using Signals;
using UnityEngine;
using Zenject;

//todo save to persistance data path
//todo crypt save data
public class PlayerProfileManager
{
    private const string PROGRESS = "ProgressPrefs";
    private PlayerProfile profile;
    [Inject] private SignalBus bus;

    
    public void Load()
    {
        string str = PlayerPrefs.GetString(PROGRESS);
        profile = new PlayerProfile();
        if (!string.IsNullOrEmpty(str))
        {
            profile = JObject.Parse(str).ToObject<PlayerProfile>();
        }
    }

    public void SetPostProcessingState(bool state)
    {
        if (profile == null) //in case we launch game without preloader
        {
            return;
        }
        if (profile.Settings.PostProcessing != state)
        {
            profile.Settings.PostProcessing = state;
            Save();
            bus.Fire<OnPostProcessingSettingsChangedSignal>();
        }
    }

    public void ChangePostProcessingState()
    {
        SetPostProcessingState(!GetPostProcessingState());
    }
    
    public bool GetPostProcessingState()
    {
        if (profile == null) //in case we launch game without preloader
        {
            return true;
        }
        return profile.Settings.PostProcessing;
    }

    private void Save()
    {
        PlayerPrefs.SetString(PROGRESS, JObject.FromObject(profile).ToString());
    }

    public void ChangeCameraModeState()
    {
        if (profile == null) //in case we launch game without preloader
        {
            return;
        }

        switch (GetCameraModeState())
        {
            case CameraMode.Cinemachine:
                ChangeCameraModeState(CameraMode.Simple);
                break;
            case CameraMode.Simple:
                ChangeCameraModeState(CameraMode.Fallow);
                break;
            case CameraMode.Fallow:
                ChangeCameraModeState(CameraMode.Cinemachine);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeCameraModeState(CameraMode cameraMode)
    {
        if (profile.Settings.CameraMode != cameraMode)
        {
            profile.Settings.CameraMode = cameraMode;
            Save();
            bus.Fire<OnCameraModeChangedSignal>();
        }
    }

    public CameraMode GetCameraModeState()
    {
        if (profile == null) //in case we launch game without preloader
        {
            return CameraMode.Cinemachine;
        }
        return profile.Settings.CameraMode;
    }
    
    public PlayerProfile.PlayerProgress GetProgress()
    {
        return profile.Progress;
    }

    public void OnLevelComplete(int levelNum, int startsCount)
    {
        Debug.Log("LevelCompleted! Num: " + levelNum + " starts: " + startsCount);
        if (profile.Progress.levels.Count < levelNum + 1)
        {
            profile.Progress.levels.Add(new PlayerProfile.PlayerProgress.LevelProgress());
        }

        if (profile.Progress.levels[levelNum].StarsCount < startsCount)
        {
            profile.Progress.levels[levelNum].StarsCount = startsCount;
        }
        Save();
    }
}

public class PlayerProfile
{
    public PlayerSettings Settings = new PlayerSettings();
    public PlayerProgress Progress = new PlayerProgress();

    public class PlayerSettings
    {
        public bool PostProcessing = true;
        public CameraMode CameraMode = CameraMode.Cinemachine;
    }

    public class PlayerProgress
    {
        public List<LevelProgress> levels = new List<LevelProgress>();
        
        public class LevelProgress
        {
            public int StarsCount;
            public GhostData Ghost = new GhostData();
            
            public class GhostData
            {
                
            }
        }
    }
}