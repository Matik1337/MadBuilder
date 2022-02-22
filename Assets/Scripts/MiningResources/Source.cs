using System.Collections;
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

    [Space(10)] [Header("Collapse Properties")] 
    
    [SerializeField] private float _maxScale = 1.1f;
    [SerializeField] private float _maxScaleDelay = .2f;
    [SerializeField] private float _minScaleDelay = .5f;

    [Space(10)] [Header("Effects")]
    
    [SerializeField] private ParticleSystem _collapseFX;

    private bool _isMined;

    public UnityAction Collapsed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !_isMined)
        {
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
        
        for (int i = 0; i < _amount; i++)
        {
            var spawned = Instantiate(_currentType, transform.position, Quaternion.identity);
            
            spawned.Move(inventory, _spawnRange);
            
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
