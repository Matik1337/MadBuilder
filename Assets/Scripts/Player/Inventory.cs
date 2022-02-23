using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private float _packingStep;
    [SerializeField] private Player _player;

    [SerializeField] private int _maxTowerSize;
    
    private List<SourceType> _resources;
    
    public IEnumerable<SourceType> Resources => _resources;
    public UnityAction<string> Added;

    private void Awake()
    {
        _resources = new List<SourceType>();
    }

    public void Add(SourceType resource)
    {
        _resources.Insert(0, resource);
        Added?.Invoke(resource.Type);
    }

    public Vector3 GetTargetPosition(string type)
    {
        if (type == Constants.Resources.Wood)
        {
            return CalculateTargetPosition();
        }

        if (type == Constants.Resources.Stone)
        {
            return CalculateTargetPosition();
        }
        
        throw new ArgumentException();
    }

    private Vector3 CalculateTargetPosition()
    {
        int count = _resources.Count;
        int columnNumber = count / _maxTowerSize;

        int y = count - _maxTowerSize * columnNumber;

        return new Vector3(0, y* _packingStep, -_packingStep * (columnNumber + 1) + _packingStep / 2);
    }

    private int GetCountOfType(string type)
    {
        return _resources.Count(resource => resource.Type == type);
    }
}
