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
        [Inject] private Settings settings;
        
        
        public void LoadLevel(int levelNum)
        {
            //todo show loading screen
            //wati sec
            zenjectSceneLoader.LoadScene(levelSceneName, LoadSceneMode.Single, container =>
            {
                container.Bind<int>().WithId("levelNum").FromInstance(levelNum);
                container.Bind<GameObject>().WithId("bindedObject").FromInstance(settings.GetObjectById(levelNum));
            });
            //wati sec
            //Hide loading screen

            //fire singal
        }

        public void LoadMainMenu()
        {
            zenjectSceneLoader.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}