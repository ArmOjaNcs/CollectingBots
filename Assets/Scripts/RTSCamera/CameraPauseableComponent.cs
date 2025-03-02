using Zenject;

public abstract class CameraPauseableComponent : PauseableObject
{
    [Inject]
    private void Construct(Pause pause)
    {
        pause.Register(this);
    }
}