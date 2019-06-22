using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
    public List<LevelSettings> Levels = new List<LevelSettings>();
}

public class LevelSettings
{
    public float OneStarTime;
    public float TwoStarsTime;
    public float ThreeStarsTime;

    public string SceneName;
}