using DefaultNamespace;
using Zenject;

public class PreloaderManager
{
    [Inject] private PlayerProfileManager playerProfileManager;
    [Inject] private LoaderViewManager loaderViewManager;

    public void Preloading()
    {
        playerProfileManager.Load(() =>
        {
            loaderViewManager.LoadMainMenu();
        });
    }
}
