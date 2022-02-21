using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private float _packingStep;
    [SerializeField] private Player _player;
    
    private List<SourceType> _resources;
    
    public IEnumerable<SourceType> Resources => _resources;

    private void Awake()
    {
        _resources = new List<SourceType>();
    }

    private void OnEnable()
    {
        _player.Lost += OnLost;
    }

    private void OnLost()
    {
        Invoke(nameof(Collapse), .5f);
    }

    private void Collapse()
    {
        return;
        
        foreach (var resource in _resources)
        {
            resource.transform.SetParent(null);
            resource.transform.position = transform.position;
            //resource.Explode(Vector3.zero, 0f);
        }
    }

    public void Add(SourceType resource)
    {
        _resources.Insert(0, resource);
    }

    public Vector3 GetTargetPosition()
    {
        return new Vector3(0, _resources.Count * _packingStep, 0);
    }
}
