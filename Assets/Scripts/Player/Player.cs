using System;
using System.Collections;
using Cinemachine;
using RunnerMovementSystem;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InputTransformation))]
[RequireComponent(typeof(PlayerAnimatorHolder))]
[RequireComponent(typeof(MovementSystem))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _miningSpeed;
    [SerializeField] private float _changeStep;
    [SerializeField] private float _miningDelay;
    [SerializeField] private float _deathDelay;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private ParticleSystem _swordFX;

    [Space(10)] [Header("Tools")] 
    
    [SerializeField] private Tool _tool;
    
    private InputTransformation _input;
    private PlayerAnimatorHolder _animatorHolder;
    private MovementSystem _movementSystem;

    public UnityAction Won;
    public UnityAction Lost;
    
    private void Awake()
    {
        _input = GetComponent<InputTransformation>();
        _animatorHolder = GetComponent<PlayerAnimatorHolder>();
        _movementSystem = GetComponent<MovementSystem>();
    }

    private void Start()
    {
        Run(false);
    }

    private IEnumerator AnimateMining(string toolName)
    {
        _animatorHolder.SetAnimation(Constants.Animations.Fight, 0);
        StartCoroutine(ChangeSpeed(_miningSpeed));
        _tool.Enable(toolName, true);
        yield return new WaitForSeconds(_miningDelay / 2);
        
        _swordFX.Play();
        yield return new WaitForSeconds(_miningDelay / 2);
        
        StopCoroutine(ChangeSpeed(_runSpeed));
        StartCoroutine(ChangeSpeed(_runSpeed));
        _tool.Enable(toolName, false);
    }

    private IEnumerator ChangeSpeed(float targetValue)
    {
        while (_movementSystem.CurrentSpeed != targetValue)
        {
            _movementSystem.SetSpeed(Mathf.MoveTowards(_movementSystem.CurrentSpeed, targetValue, _changeStep));
            yield return null;
        }
    }
    
    public void Die()
    {
        _input.EnableMovement(false);
        _animatorHolder.SetAnimation(Constants.Animations.Death, _deathDelay);
        _camera.Follow = null;
        _camera.LookAt = null;
        Lost?.Invoke();
    }

    public void Jump()
    {
        _animatorHolder.SetAnimation(Constants.Animations.Jump, 0);
    }

    public void Mine(string toolName)
    {
        StartCoroutine(AnimateMining(toolName));
    }

    public void Run(bool needRun)
    {
        _animatorHolder.SetMoveState(needRun);

        if (needRun)
            _movementSystem.SetSpeed(_runSpeed);
        else
            _movementSystem.SetSpeed(0);
    }

    public void Victory()
    {
        _animatorHolder.SetAnimation(Constants.Animations.Victory, 0);
        Won?.Invoke();
    }

    public void Defeat()
    {
        _animatorHolder.SetAnimation(Constants.Animations.Defeat, 0);
        Lost?.Invoke();
    }
}
