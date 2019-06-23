using DefaultNamespace;
using Signals;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerProfileManager>().AsSingle();
        Container.Bind<LoaderViewManager>().AsSingle();
        Container.Bind<string>().WithId("LevelSceneName").FromInstance("LevelScene").WhenInjectedInto<LoaderViewManager>();
        Container.Bind<string>().WithId("MainMenuSceneName").FromInstance("MainMenuScene").WhenInjectedInto<LoaderViewManager>();
        Container.Bind<UISoundManager>().FromInstance(FindObjectOfType<UISoundManager>()).AsSingle();
        
        InstallSignals();
        
        //do it to make possible to run scene without all another game
        Container.Bind<LevelSettings>().FromInstance(new LevelSettings()
            {OneStarTime = 10, TwoStarsTime = 20, ThreeStarsTime = 300});
    }

    private void InstallSignals()
    {
        Container.DeclareSignal<OnCheckpointAchievedSignal>();
        Container.DeclareSignal<OnLoseCheckpointSignal>();
        Container.DeclareSignal<OnRaceStartSignal>();
        Container.DeclareSignal<OnStarFailedSignal>();
        Container.DeclareSignal<OnLevelFailedSignal>();
        Container.DeclareSignal<OnFinishAchievedSignal>();
        Container.DeclareSignal<OnLevelFinishedSignal>();
        Container.DeclareSignal<OnTakeCheckpointSignal>();
        Container.DeclareSignal<OnPostProcessingSettingsChangedSignal>();
        Container.DeclareSignal<OnCameraModeChangedSignal>();

        SignalBusInstaller.Install(Container);
    }
}
