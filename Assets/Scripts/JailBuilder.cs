using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class JailBuilder : MonoBehaviour
{
    [SerializeField] private Material _transparentMaterial;
    [SerializeField] private float _moveDelay;
    [SerializeField] private float _explosionPower;
    
    private List<SourceType> _items;
    private List<SourceType> _placedItems;
    private Player _player;
    private Inventory _inventory;
    
    public UnityAction BuildStarted;
    public UnityAction<float> Placed;
    public UnityAction<bool> BuildFinished;
    
    private void Awake()
    {
        _items = GetComponentsInChildren<SourceType>().ToList();
        _placedItems = new List<SourceType>();
    }
    
    private void Start()
    {
        SetMaterial();
    }

    private void SetMaterial()
    {
        foreach (var item in _items)
        {
            item.SetMaterial(_transparentMaterial);
        }
    }

    public void StartBuild(Inventory inventory)
    {
        _inventory = inventory;
        
        BuildStarted?.Invoke();
        StartCoroutine(Build());
    }

    private IEnumerator Build()
    {
        var recources = _inventory.Resources.ToList();

        foreach (var sourceType in _items)
        {
            if (Contains(recources, sourceType))
            {
                var current = Get(recources, sourceType).transform;
                var target = sourceType.transform;
                
                current.DOLocalMove(target.localPosition, _moveDelay);
                current.DORotateQuaternion(target.rotation, _moveDelay);
                current.DOScale(target.localScale, _moveDelay);
                
                yield return new WaitForSeconds(_moveDelay/2);

                sourceType.transform.localScale = Vector3.zero;
                Placed?.Invoke((float)_placedItems.Count / _items.Count * 100f);
            }
        }

        bool result = _placedItems.Count == _items.Count;
        
        BuildFinished?.Invoke(result);
        
        if (!result)
        {
            yield return new WaitForSeconds(_moveDelay);
            Collapse();
        }
    }

    public void Collapse()
    {
        Vector3 startPoint = GetAveragePosition();

        foreach (var item in _placedItems)
        {
            item.Explode((item.transform.position - startPoint).normalized, _explosionPower);
        }
    }

    private Vector3 GetAveragePosition()
    {
        Vector3 position = Vector3.zero;
        
        foreach (var item in _placedItems)
        {
            position += item.transform.position;
        }
        
        position /= _placedItems.Count;

        return position;
    }


    private bool Contains(List<SourceType> resources, SourceType current)
    {
        return resources.Exists(item => item.Type == current.Type);
    }

    private SourceType Get(List<SourceType> resources, SourceType current)
    {
        var result = resources.First(item => item.Type == current.Type);
        result.transform.SetParent(transform);
        resources.Remove(result);
        _placedItems.Add(result);
        _inventory.Remove(result);
        return result;
    }
}
