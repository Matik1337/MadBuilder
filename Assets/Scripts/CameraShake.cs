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
    
    private CinemachineVirtualCamera _camera;
    private CinemachineBasicMultiChannelPerlin _perlin;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _perlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _perlin.m_AmplitudeGain = 0;
    }

    private void OnEnable()
    {
        foreach (var source in _sources)
        {
            source.Collapsed += OnCollapsed;
        }

        _houseBuilder.BuildStarted += OnBuildStarted;
    }

    private void OnDisable()
    {
        foreach (var source in _sources)
        {
            source.Collapsed -= OnCollapsed;
        }
        
        _houseBuilder.BuildStarted -= OnBuildStarted;
    }

    private void OnBuildStarted()
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
