using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DefaultNamespace
{
    public class LoaderViewManager
    {
        [Inject] private ZenjectSceneLoader zenjectSceneLoader;
        
        [Inject(Id = "LevelSceneName")] private string levelSceneName;
        [Inject(Id = "MainMenuSceneName")] private string mainMenuSceneName;
        [Inject] private GameSettings gameSettings;
        private int lastLevelNum;
        
        public void LoadLevel(int levelNum)
        {
            //wait sec
            //show loading screen
            lastLevelNum = levelNum;
            zenjectSceneLoader.LoadScene(levelSceneName, LoadSceneMode.Single, container =>
            {
                container.Bind<int>().WithId("levelNum").FromInstance(levelNum);
                container.Rebind<LevelSettings>().FromInstance(gameSettings.Levels[levelNum]);
            });
            //wait sec
            //Hide loading screen

            //fire signal
        }
        
        public void ReloadLevel()
        {
            LoadLevel(lastLevelNum);
        }

        public void LoadMainMenu()
        {
            zenjectSceneLoader.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}