using System;
using System.Collections;
using Cinemachine;
using Extensions;
using RunnerMovementSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputTransformation))]
[RequireComponent(typeof(PlayerAnimatorHolder))]
[RequireComponent(typeof(MovementSystem))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _miningSpeed;
    [SerializeField] private float _miningDelay;
    [SerializeField] private float _deathDelay;
    [SerializeField] private float _fightSpeed;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private ParticleSystem _swordFX;

    [Space(10)] [Header("Tools")] 
    
    [SerializeField] private Tool _tool;
    
    private InputTransformation _input;
    private PlayerAnimatorHolder _animatorHolder;
    private MovementSystem _movementSystem;
    private float _startTime;

    public UnityAction Won;
    public UnityAction Lost;
    
    private void Awake()
    {
        _startTime = Time.time;
        _input = GetComponent<InputTransformation>();
        _animatorHolder = GetComponent<PlayerAnimatorHolder>();
        _movementSystem = GetComponent<MovementSystem>();
        Amplitude.Instance.LogLevelStart(SceneManager.GetActiveScene().buildIndex);
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
        _movementSystem.SetSpeed(targetValue);
        
        yield break;
    }
    
    public void SetDefaultSpeed()
    {
        StartCoroutine(ChangeSpeed(_runSpeed));
    }

    public void SetFightSpeed()
    {
        StartCoroutine(ChangeSpeed(_fightSpeed));
    }
    
    public void Die()
    {
        _input.EnableMovement(false);
        _animatorHolder.SetAnimation(Constants.Animations.Death, _deathDelay);
        _camera.Follow = null;
        _camera.LookAt = null;
        
        Amplitude.Instance.LogLevelFail(SceneManager.GetActiveScene().buildIndex, 
            AmplitudeEvents.Reasons.DeadFromEnemy, (int)(Time.time - _startTime));
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
        _input.EnableMovement(needRun);

        if (needRun)
            _movementSystem.SetSpeed(_runSpeed);
        else
            _movementSystem.SetSpeed(0);
    }

    public void Victory()
    {
        _animatorHolder.SetAnimation(Constants.Animations.Victory, 0);
        Amplitude.Instance.LogLevelComplete(SceneManager.GetActiveScene().buildIndex, (int)(Time.time - _startTime));
        Won?.Invoke();
    }

    public void Defeat()
    {
        Run(false);
        _animatorHolder.SetAnimation(Constants.Animations.Defeat, 0);
        Lost?.Invoke();
    }
}
