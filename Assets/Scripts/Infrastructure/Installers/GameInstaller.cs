using Infrastructure;
using Infrastructure.Level.EventsBus;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        CreateEventBus();
        CreateSceneLoader();
    }
    private void CreateEventBus()
    {
         Container.BindInterfacesAndSelfTo<EventBus>().AsSingle();
    }

    private void CreateSceneLoader()
    {
        Container.Bind<SceneLoader>().AsSingle().NonLazy();
        
    }
}