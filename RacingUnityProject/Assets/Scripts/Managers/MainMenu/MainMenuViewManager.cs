using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class MainMenuViewManager : MonoBehaviour
{
    [Inject] private LoaderViewManager loaderViewManager;
    
    [UsedImplicitly]
    public void OnLevelSelected(int levelNum)
    {
        loaderViewManager.LoadLevel(levelNum);
    }
}
