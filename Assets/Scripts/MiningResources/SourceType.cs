using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class SourceType : MonoBehaviour
{
    [SerializeField] private string _type;
    [SerializeField] private string _tool;
    [SerializeField] private Transform _upperPoint;

    private float _moveToRandPosDelay = .5f;
    private float _moveToPlayerStep = .05f;
    private float _maxScale = 3f;
    private Rigidbody _rigidbody;
    private MeshRenderer[] _meshRenderers;

    public string Type => _type;
    public string Tool => _tool;
    public Vector3 UpperPoint => _upperPoint.position;

    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public void Move(Inventory inventory, float range)
    {
        StartCoroutine(MoveToRandPos(inventory, range));
    }

    private IEnumerator MoveToRandPos(Inventory inventory, float range)
    {
        Vector3 randomPosition = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));

        transform.DOMove(transform.position + randomPosition, _moveToRandPosDelay);
        transform.DOScale(transform.localScale * _maxScale, _moveToRandPosDelay);
        
        yield return new WaitForSeconds(_moveToRandPosDelay);
        
        StartCoroutine(MoveToPlayer(inventory));
    }

    private IEnumerator MoveToPlayer(Inventory inventory)
    {
        transform.SetParent(inventory.transform);
        inventory.Add(this);
        Vector3 targetPosition = inventory.GetTargetPosition();
        
        transform.DOScale(transform.localScale / _maxScale, _moveToRandPosDelay);
        
        while (transform.localPosition != targetPosition)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, _moveToPlayerStep);
            yield return null;
        }
    }

    public void Explode(Vector3 direction, float power)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce(direction * power, ForceMode.VelocityChange);
    }

    public void SetMaterial(Material material)
    {
        foreach (var renderer in _meshRenderers)
        {
            renderer.material = material;
        }
    }
}