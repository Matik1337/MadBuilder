using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSlider : MonoBehaviour
{
    [SerializeField] private HouseBuilder _houseBuilder;
    [SerializeField] private ResourcesDispalyer _resourcesDispalyer;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _maxScale = 1.2f;
    [SerializeField] private float _animationDelay = .5f;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        OnPlaced(0);
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        _houseBuilder.BuildStarted += OnBuildStarted;
        _houseBuilder.BuildFinished += OnBuildFinished;
        _houseBuilder.Placed += OnPlaced;
    }

    private void OnDisable()
    {
        _houseBuilder.BuildStarted -= OnBuildStarted;
        _houseBuilder.BuildFinished -= OnBuildFinished;
        _houseBuilder.Placed -= OnPlaced;
    }

    private void OnBuildStarted()
    {
        StartCoroutine(Enable());
    }

    private void OnBuildFinished()
    {
        StartCoroutine(Enable());
    }

    private void OnPlaced(float percents)
    {
        _slider.value = Mathf.RoundToInt(percents);
        _text.text = _slider.value + "%";
    }

    private IEnumerator Enable()
    {
        _resourcesDispalyer.transform.DOScale(Vector3.zero, _animationDelay);
        transform.DOScale(Vector3.one * _maxScale, _animationDelay);
        yield return new WaitForSeconds(_animationDelay);
        transform.DOScale(Vector3.one, _animationDelay);
    }
}
