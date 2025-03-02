using Zenject;

public class PickedResourcesHolderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PickedResourcesHandler>().FromNew().AsSingle().NonLazy();
    }
}