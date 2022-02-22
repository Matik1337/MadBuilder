using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RunnerMovementSystem;
using TMPro;
using UnityEngine;

public class TapToStart : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _text;

    private VariableJoystick _joystick;
    private float _maxScale = .3f;
    private float _animationDelay = .3f;
    private bool _isAnimating;

    private void Awake()
    {
        _joystick = GetComponentInChildren<VariableJoystick>();
    }

    private void Start()
    {
        StartCoroutine(Animate());
    }

    private void OnEnable()
    {
        _joystick.Down += OnDown;
    }

    private void OnDown()
    {
        _joystick.Down -= OnDown;
        _isAnimating = false;
        _player.Run(true);
    }

    private IEnumerator Animate()
    {
        _isAnimating = true;

        while (_isAnimating)
        {
            _text.transform.localScale = Vector3.one + Vector3.one * Mathf.PingPong(Time.time, _maxScale);
            yield return null;
        }
        
        _text.transform.DOScale(Vector3.zero, _animationDelay);
    }
}
