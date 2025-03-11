using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class AddresablesScript : MonoBehaviour
{
    [SerializeField] private AssetReference _loadableMat;
    [SerializeField] private Ground _ground;

    private async void Start()
    {
        AsyncOperationHandle<Material> handle = _loadableMat.LoadAssetAsync<Material>();
        await handle.Task;

        Renderer renderer = _ground.GetComponent<Renderer>();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Material material = handle.Result;
            renderer.material = material;
            Addressables.Release(handle);
        }
    }
}
