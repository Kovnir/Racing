using DefaultNamespace;
using Signals;
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
        
        //do it to make possible to run scene without all another game
        Container.Bind<LevelSettings>().FromInstance(new LevelSettings()
            {OneStarTime = 10, TwoStarsTime = 20, ThreeStarsTime = 30});
    }

    private void InstallSignals()
    {
        Container.DeclareSignal<OnCheckpointAchievedSignal>();
        Container.DeclareSignal<OnLoseCheckpointSignal>();
        Container.DeclareSignal<OnRaceStartSignal>();
        Container.DeclareSignal<OnStarFailedSignal>();
        Container.DeclareSignal<OnLevelFailedSignal>();
        
        SignalBusInstaller.Install(Container);
    }
}
