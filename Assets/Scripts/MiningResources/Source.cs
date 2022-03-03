using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Source : MonoBehaviour
{
    [SerializeField] private SourceType _currentType;
    [SerializeField] private int _amount;
    [SerializeField] private float _spawnRange;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private float _startSpawnDelay;
    [SerializeField] private Transform _image;

    [Space(10)] [Header("Collapse Properties")] 
    
    [SerializeField] private float _maxScale = 1.1f;
    [SerializeField] private float _maxScaleDelay = .2f;
    [SerializeField] private float _minScaleDelay = .5f;

    [Space(10)] [Header("Effects")]
    
    [SerializeField] private ParticleSystem _collapseFX;

    private bool _isMined;
    private List<SourceType> _spawned;

    public UnityAction Collapsed;

    private void Awake()
    {
        _spawned = new List<SourceType>();
        
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < _amount; i++)
        {
            _spawned.Add(Instantiate(_currentType, transform.position - Vector3.up, Quaternion.identity));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !_isMined)
        {
            _image.DOScale(Vector3.zero, _minScaleDelay);
            _isMined = true;
            player.Mine(_currentType.Tool);
            StartCoroutine(SpawnResources(player.GetComponentInChildren<Inventory>()));
        }
    }

    private IEnumerator SpawnResources(Inventory inventory)
    {
        yield return new WaitForSeconds(_startSpawnDelay);
        Collapsed?.Invoke();
        yield return new WaitForSeconds(_spawnDelay);

        foreach (var item in _spawned)
        {
            item.Move(inventory, _spawnRange);
            
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
    
    private IEnumerator Collapse()
    {
        transform.DOScale(Vector3.one * _maxScale, _maxScaleDelay);
        
        yield return new WaitForSeconds(_maxScaleDelay);

        Instantiate(_collapseFX, transform.position, Quaternion.identity).Play();
        transform.DOScale(Vector3.zero, _minScaleDelay);

        yield return new WaitForSeconds(_minScaleDelay);

        gameObject.SetActive(false);
    }
}
