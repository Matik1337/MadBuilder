using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ToolsDataBase), menuName = "MiningResounces/DataBase", order = 51)]
public class ToolsDataBase : ScriptableObject
{
    [SerializeField] private List<BaseElement> _baseElements;

    public IEnumerable<BaseElement> Data => _baseElements;

    public string GetTool(string resource)
    {
        if (ContainsResource(resource))
            return _baseElements.First(element => element.Resource == resource).Tool;
        
        throw new ArgumentException();
    }

    public string GetResource(string tool)
    {
        if(ContainsTool(tool))
            return _baseElements.First(element => element.Resource == tool).Resource;

        throw new ArgumentException();
    }

    public bool ContainsTool(string tool)
    {
        foreach (var element in _baseElements)
        {
            if (element.Tool == tool)
                return true;
        }

        return false;
    }
    
    public bool ContainsResource(string resource)
    {
        foreach (var element in _baseElements)
        {
            if (element.Resource == resource)
                return true;
        }

        return false;
    }

    public void Add(BaseElement newElement)
    {
        _baseElements.Add(newElement);
    }

    public void Remove(BaseElement currentElement)
    {
        _baseElements.Remove(currentElement);
    }
}

[Serializable]
public struct BaseElement
{
    [SerializeField] private string _resource;
    [SerializeField] private string _tool;
    
    public string Resource => _resource;
    public string Tool => _tool;
}