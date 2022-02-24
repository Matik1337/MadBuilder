using System;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private JailBuilder _jailBuilder;
    [SerializeField] private float _deathDelay;

    private Animator _animator;
    private bool _isWorked;
    private Player _player;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _jailBuilder = GetComponentInChildren<JailBuilder>();
    }

    private void OnEnable()
    {
        _jailBuilder.BuildFinished += OnBuildFinished;
    }

    private void OnDisable()
    {
        _jailBuilder.BuildFinished -= OnBuildFinished;
    }

    private void OnBuildFinished(bool sucsess)
    {
        if (sucsess)
        {
            _animator.SetTrigger(Constants.Animations.Defeat);
            _player.SetDefaultSpeed();
            Collapse();
        }
        else
        {
            Invoke(nameof(Fight), _deathDelay);
        }
    }

    private void Fight()
    {
        _animator.SetTrigger(Constants.Animations.Fight);
        _player.Die();
    }

    public void Collapse()
    {
        _jailBuilder.Collapse();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !_isWorked)
        {
            _isWorked = false;
            _player = player;
            _player.SetFightSpeed();
            _jailBuilder.StartBuild(_player.GetComponentInChildren<Inventory>());
        }
    }
}
