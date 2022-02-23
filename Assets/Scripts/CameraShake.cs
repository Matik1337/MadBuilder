using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private HouseBuilder _houseBuilder;
    [SerializeField] private Source[] _sources;

    [SerializeField] private float _noiseDelay;
    [SerializeField] private Vector3 _defaultOffcet;
    [SerializeField] private Vector3 _buildOffcet;

    private CinemachineVirtualCamera _camera;
    private CinemachineBasicMultiChannelPerlin _perlin;
    private CinemachineTransposer _transposer;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _perlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        _perlin.m_AmplitudeGain = 0;
        _transposer.m_FollowOffset = _defaultOffcet;
    }

    private void OnEnable()
    {
        foreach (var source in _sources)
        {
            source.Collapsed += OnCollapsed;
        }

        _houseBuilder.BuildStarted += OnBuildStarted;
        _houseBuilder.BuildFinished += OnBuildFinished;
    }

    private void OnDisable()
    {
        foreach (var source in _sources)
        {
            source.Collapsed -= OnCollapsed;
        }
        
        _houseBuilder.BuildStarted -= OnBuildStarted;
        _houseBuilder.BuildFinished -= OnBuildFinished;
    }

    private void OnBuildStarted()
    {
        _camera.Priority = 0;
        //_camera.Follow = _houseBuilder.transform;
        //_camera.LookAt = _houseBuilder.transform;
        //_transposer.m_FollowOffset = _buildOffcet;
    }

    private void OnBuildFinished()
    {
        _camera.Follow = null;
        _camera.LookAt = null;
    }

    private void OnCollapsed()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        _perlin.m_AmplitudeGain = 1f;
        yield return new WaitForSeconds(_noiseDelay);
        _perlin.m_AmplitudeGain = 0f;
    }
}
