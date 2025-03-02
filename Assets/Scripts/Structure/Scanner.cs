using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Scanner : MonoBehaviour
{
    private HashSet<Resource> _scannedResources = new();
    private BoxCollider _collider;
    private WaitForSeconds _colliderLifeTime;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        _colliderLifeTime = new WaitForSeconds(GameUtils.ScannerColliderLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _scannedResources.Add(resource);
            resource.Released += OnResourceReleased;
        }  
    }

    public HashSet<Resource> ScanArea()
    {
        _collider.enabled = true;
        StartCoroutine(DisableCollider());
        
        return _scannedResources;
    }

    private void OnResourceReleased(Resource resource)
    {
        _scannedResources.Remove(resource);
        resource.Released -= OnResourceReleased;
    }

    private IEnumerator DisableCollider()
    {
        yield return _colliderLifeTime;
        _collider.enabled = false;
    }
}