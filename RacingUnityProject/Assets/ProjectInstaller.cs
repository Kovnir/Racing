using DefaultNamespace;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerProfileManager>().AsSingle();
        Container.Bind<LoaderViewManager>().AsSingle();
        Container.Bind<string>().WithId("LevelSceneName").FromInstance("LevelScene").WhenInjectedInto<LoaderViewManager>();
        Container.Bind<string>().WithId("MainMenuSceneName").FromInstance("MainMenuScene").WhenInjectedInto<LoaderViewManager>();
        
        Container.DeclareSignal<SomeSignal>();

        
        SignalBusInstaller.Install(Container);
        
        //
//        Container.Bind<>()
    }
}
