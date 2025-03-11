using System.Collections.Generic;
using System.Linq;

public class PickedResourcesHandler 
{
    private HashSet<Resource> _pickedResources = new();

    public void AddPickedResource(Resource resource)
    {
        _pickedResources.Add(resource);
    }

    public IEnumerable<Resource> GetAvailableResources(HashSet<Resource> resources)
    {
        IEnumerable<Resource> availableResources = from resource in resources
                              where _pickedResources.Contains(resource) == false
                              select resource;
       
        return availableResources;
    }

    public void RemoveReleasedResource(Resource resource)
    {
        _pickedResources.Remove(resource);
    }
}