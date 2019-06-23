using UnityEditor;
using UnityEditor.SceneManagement;

public static class ScenesOpener
{
    [MenuItem("Racing/Open Scene/Preloader")]
    public static void OpenPreloader()
    {
        Open("PreloaderScene");
    }
    [MenuItem("Racing/Open Scene/MainMenu")]
    public static void OpenMainMenu()
    {
        Open("MainMenuScene");        
    }
    [MenuItem("Racing/Open Scene/Levels/Level1")]
    public static void OpenLevel1()
    {
        Open("Level1");
    }
    [MenuItem("Racing/Open Scene/Levels/Level2")]
    public static void OpenLevel2()
    {
        Open("Level2");
    }
    [MenuItem("Racing/Open Scene/Levels/Level3")]
    public static void OpenLevel3()
    {
        Open("Level3");
    }

    private static void Open(string sceneName)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
    }
}
