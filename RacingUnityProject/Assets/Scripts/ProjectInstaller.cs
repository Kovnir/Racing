using DefaultNamespace;
using Singals;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerProfileManager>().AsSingle();
        Container.Bind<LoaderViewManager>().AsSingle();
        Container.Bind<string>().WithId("LevelSceneName").FromInstance("LevelScene").WhenInjectedInto<LoaderViewManager>();
        Container.Bind<string>().WithId("MainMenuSceneName").FromInstance("MainMenuScene").WhenInjectedInto<LoaderViewManager>();
        
        InstallSignals();
    }

    private void InstallSignals()
    {
        Container.DeclareSignal<OnCheckpointAchievedSignal>();
        Container.DeclareSignal<OnLoseCheckpointSignal>();
        Container.DeclareSignal<OnRaceStartSignal>();
        
        SignalBusInstaller.Install(Container);
    }
}
