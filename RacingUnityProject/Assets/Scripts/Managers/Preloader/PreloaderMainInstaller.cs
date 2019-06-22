using Zenject;

namespace DefaultNamespace.Preloader
{
    public class PreloaderMainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PreloaderManager>().AsSingle();
        }
    }
}