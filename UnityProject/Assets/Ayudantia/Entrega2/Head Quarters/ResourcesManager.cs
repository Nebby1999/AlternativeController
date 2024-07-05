using System.Collections;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    [SerializeField] private Resource _redResource;
    [SerializeField] private Resource _blackResource;

    public float BlackResource => _blackResource.Value;
    public float RedResource => _redResource.Value;

    public bool LoadMaterial(MineralType mineral, float amount)
    {
        Resource targetResource = GetResourceByType(mineral);

        if (targetResource == null)
        {
            Debug.LogError("No Mineral to Load");
            return false;
        }
        AddMineral(targetResource, amount);
        return true;
    }

    private Resource GetResourceByType(MineralType mineral)
    {
        return mineral == MineralType.Black ? _blackResource : (mineral == MineralType.Red ? _redResource : null);
    }

    private void AddMineral(Resource resource, float amount)
    {
        resource.Add(amount);
        Debug.Log("Mineral Loaded: " + resource.Value);
    }

    public bool UnloadMineral(MineralType mineral, float amount)
    {
        Resource targetResource = GetResourceByType(mineral);

        if (targetResource == null || targetResource.Value < amount)
        {
            Debug.LogError("No Mineral to Unload");
            return false;
        }
        targetResource.Substract(amount);
        Debug.Log("Mineral Unloaded: " + targetResource.Value);
        return true;
    }
}