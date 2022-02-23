using System;
using RunnerMovementSystem;
using UnityEngine;

[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(PlayerAnimatorHolder))]
public class InputTransformation : MonoBehaviour
{
    [SerializeField] private VariableJoystick _joystick;
    [SerializeField] private float _offsetChangingStep;

    private MovementSystem _movementSystem;
    private PlayerAnimatorHolder _animatorHolder;
    private bool _canMove;
    private float _offset;
    private void Awake()
    {
        _movementSystem = GetComponent<MovementSystem>();
        _animatorHolder = GetComponent<PlayerAnimatorHolder>();
        _offset = 0;
    }

    private void Start()
    {
        EnableMovement(true);
    }

    private void OnEnable()
    {
        _joystick.Up += OnUp;
    }

    private void OnDisable()
    {
        _joystick.Up -= OnUp;
    }

    private void OnUp()
    {
        _offset = _movementSystem.Offset;
    }

    private void Update()
    {
        if(_canMove)
            HandleInput();
    }

    private void HandleInput()
    {
        _movementSystem.MoveForward();
        
        _movementSystem.SetOffset(_offset + _movementSystem.CurrentRoad.Width * _joystick.Horizontal);
    }

    public void EnableMovement(bool needEnable)
    {
        _canMove = needEnable;
        _animatorHolder.SetMoveState(needEnable);
    }
}
