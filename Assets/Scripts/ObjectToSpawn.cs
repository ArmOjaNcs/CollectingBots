using Zenject;

public abstract class ObjectToSpawn : PauseableObject
{
    public bool IsInitialized { get; private set; }

    public void SetInitialized()
    {
        IsInitialized = true;
    }
}