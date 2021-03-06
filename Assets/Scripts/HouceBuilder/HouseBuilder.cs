using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Extensions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HouseBuilder : MonoBehaviour
{
    [SerializeField] private Material _transparentMaterial;
    [SerializeField] private float _moveDelay;
    [SerializeField] private float _finisherDelay;
    [SerializeField] private float _explosionPower;

    private List<SourceType> _items;
    private List<SourceType> _placedItems;
    private Inventory _inventory;
    private Player _player;
    private float _maxScale = 1f;
    private bool _isWorked;

    public UnityAction BuildStarted;
    public UnityAction<float> Placed;
    public UnityAction BuildFinished;

    private void Awake()
    {
        _placedItems = new List<SourceType>();
        _items = GetComponentsInChildren<SourceType>().ToList();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !_isWorked)
        {
            _isWorked = true;
            _player = player;
            _inventory = _player.GetComponentInChildren<Inventory>();
            _player.GetComponent<InputTransformation>().EnableMovement(false);
            _player.Run(false);
            BuildStarted?.Invoke();
            StartCoroutine(Build());
        }
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
                
                current.SetParent(transform);
                current.DOMove(target.position, _moveDelay);
                current.DORotateQuaternion(target.rotation, _moveDelay);
                current.DOScale(target.localScale, _moveDelay);

                yield return new WaitForSeconds(_moveDelay / 5);

                sourceType.Disable();
                Placed?.Invoke((float)_placedItems.Count / _items.Count * 100f);
            }
        }

        BuildFinished?.Invoke();
        
        yield return new WaitForSeconds(_finisherDelay);

        if (_placedItems.Count == _items.Count)
        {
            _player.Victory();
            enabled = false;
            //platFX
        }
        else
        {
            Collapse();
            Amplitude.Instance.LogLevelFail(SceneManager.GetActiveScene().buildIndex, 
                AmplitudeEvents.Reasons.NotEnoughResourcesForHouse, (int)Time.time);
            
            _player.Defeat();
        }
    }

    private bool Contains(List<SourceType> resources, SourceType current)
    {
        return resources.Exists(item => item.Type == current.Type);
    }

    private SourceType Get(List<SourceType> resources, SourceType current)
    {
        var result = resources.First(item => item.Type == current.Type);

        resources.Remove(result);
        _inventory.Remove(result);
        _placedItems.Add(result);

        return result;
    }

    private void Collapse()
    {
        Vector3 startPoint = GetAveragePosition();

        foreach (var item in _placedItems)
        {
            item.Explode((item.transform.position - startPoint).normalized, _explosionPower);
        }

        enabled = false;
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

    public int GetResourcesCount(string type)
    {
        int result = 0;
        
        foreach (var item in _items)
        {
            if (item.Type == type)
                result++;
        }

        return result;
    }
}