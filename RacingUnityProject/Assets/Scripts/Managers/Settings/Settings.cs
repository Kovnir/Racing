using UnityEngine;

public class Settings : ScriptableObject
{
    public string SomeString;
    public GameObject Object1;
    public GameObject Object2;

    public GameObject GetObjectById(int levelNum)
    {
        switch (levelNum)
        {
            case 0: return Object1;
            case 1: return Object2;
            default: return null;
        }
    }
}