using DefaultNamespace;
using Kovnir.FastTweener;
using Zenject;

public class PreloaderManager
{
    [Inject] private PlayerProfileManager playerProfileManager;
    [Inject] private LoaderViewManager loaderViewManager;

    public void Preloading()
    {
        //wait 1 second to see preloader.
        FastTweener.Schedule(1, () => { playerProfileManager.Load(() => { loaderViewManager.LoadMainMenu(); }); });
    }
}
