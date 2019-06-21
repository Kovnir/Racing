using UnityEditor;
using UnityEditor.SceneManagement;

public static class ScenesOpener
{
    [MenuItem("Scene/Preloader")]
    public static void OpenPreloader()
    {
        Open("PreloaderScene");
    }
    [MenuItem("Scene/MainMenu")]
    public static void OpenMainMenu()
    {
        Open("MainMenuScene");        
    }
    [MenuItem("Scene/Level")]
    public static void OpenLevel()
    {
        Open("LevelScene");
    }

    private static void Open(string sceneName)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
    }
}
