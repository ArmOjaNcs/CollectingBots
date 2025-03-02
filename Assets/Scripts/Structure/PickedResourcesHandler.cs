using System.Collections.Generic;

public class PickedResourcesHandler 
{
    private HashSet<Resource> _pickedResources = new();

    public void AddPickedResource(Resource resource)
    {
        _pickedResources.Add(resource);
    }

    public HashSet<Resource> GetAvailableResources(HashSet<Resource> resources)
    {
        HashSet<Resource> availableResources = new();

        foreach(Resource resource in resources)
        {
            if(_pickedResources.Contains(resource) == false)
                availableResources.Add(resource);
        }
       
        return availableResources;
    }

    public void RemoveReleasedResource(Resource resource)
    {
        _pickedResources.Remove(resource);
    }
}