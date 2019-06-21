using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class GameHudViewManager : MonoBehaviour
{
    [InjectOptional] private LoaderViewManager loaderViewManager;

    [Inject] private SignalBus signalBus;

    [Inject] private DiContainer container;
    
    private void Awake()
    {
        signalBus.Subscribe<SomeSignal>(()=>{Debug.LogError("ONONON!!!");});
//        container.BindSignal<SomeSignal>().ToMethod(() => { });
//        container.BindSignal<SomeSignal>().ToMethod<GameHudViewManager>((manager, signal) => { });
    }
    
    [UsedImplicitly]
    public void OnBackButtonClick()
    {
        
        if (loaderViewManager != null)
        {
            loaderViewManager.LoadMainMenu();
        }
        
        signalBus.Fire<SomeSignal>();
    }
}

public class SomeSignal
{
    
}
