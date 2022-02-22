using DG.Tweening;
using UnityEngine;

public class SourceDestroyer : MonoBehaviour
{
    [SerializeField] private float _power;
    [SerializeField] private float _explodeDelay;
    [SerializeField] private float _destroyDelay;

    [SerializeField] private ParticleSystem[] _collapseFX;

    private Rigidbody[] _rigidbodies;
    private Source _source;

    private void Awake()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _source = GetComponent<Source>();
        
        Fix();
    }

    private void OnEnable()
    {
        _source.Collapsed += OnCollapsed;
    }

    private void OnDisable()
    {
        _source.Collapsed -= OnCollapsed;
    }

    private void OnCollapsed()
    {
        Invoke(nameof(Explode), _explodeDelay);
    }

    private void Fix()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
    }

    private void Explode()
    {
        Vector3 avgPos = GetAveragePosition();

        foreach (var fx in _collapseFX)
        {
            Instantiate(fx, transform.position, Quaternion.identity);
        }
        
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;

            Vector3 direction = (rigidbody.transform.position - avgPos).normalized;

            rigidbody.AddForce(direction * _power, ForceMode.VelocityChange);
            rigidbody.transform.DOScale(Vector3.zero, _destroyDelay);
            Destroy(rigidbody.gameObject, _destroyDelay);
        }
    }

    private Vector3 GetAveragePosition()
    {
        Vector3 result = Vector3.zero;

        foreach (var rigidbody in _rigidbodies)
        {
            result += rigidbody.transform.position;
        }

        result /= _rigidbodies.Length;

        return result;
    }
}
