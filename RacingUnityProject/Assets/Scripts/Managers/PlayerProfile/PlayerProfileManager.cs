using System.Collections.Generic;
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

}

public class PlayerProfile
{
    public PlayerSettings Settings = new PlayerSettings();
    public PlayerProgress Progress = new PlayerProgress();

    public class PlayerSettings
    {
        public bool PostProcessing = true;
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