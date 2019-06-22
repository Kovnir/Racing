using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class GameSettings : ScriptableObject, ISerializationCallbackReceiver
{
    public List<LevelSettings> Levels = new List<LevelSettings>();

    [JsonProperty] 
    private string json;
    
    public void OnBeforeSerialize()
    {
        return;
        json = JToken.FromObject(Levels).ToString();
    }

    public void OnAfterDeserialize()
    {
        return;
        if (json != null)
        {
            Levels = JToken.FromObject(Levels).ToObject<List<LevelSettings>>();
        }

        if (Levels == null)
        {
            Levels = new List<LevelSettings>();
        }
    }
}

[Serializable]
public class LevelSettings
{
    public float OneStarTime;
    public float TwoStarsTime;
    public float ThreeStarsTime;

    public string SceneName;
}