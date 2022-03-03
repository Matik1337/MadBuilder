using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ResourcesDispalyer : MonoBehaviour
{
    [SerializeField] private HouseBuilder _houseBuilder;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private TMP_Text _woodText;
    [SerializeField] private TMP_Text _stoneText;
    [SerializeField] private float _animationDelay = .5f;
    [SerializeField] private float _maxScale = 1.2f;

    private int _currentWood = 0;
    private int _currentStone = 0;
    private int _targetWood;
    private int _targetStone;

    private void OnEnable()
    {
        _inventory.Added += OnAdded;
        _inventory.Removed += OnRemoved;
    }

    private void OnDisable()
    {
        _inventory.Added -= OnAdded;
        _inventory.Removed -= OnRemoved;
    }

    private void OnRemoved(string type)
    {
        ChangeValues(type, -1);
    }

    private void OnAdded(string type)
    {
        ChangeValues(type, 1);
    }

    private void ChangeValues(string type, int delta)
    {
        TMP_Text text;
        int current;
        int target;

        if (type == Constants.Resources.Wood)
        {
            _currentWood += delta;
            text = _woodText;
            current = _currentWood;
            target = _targetWood;
        }
        else if (type == Constants.Resources.Stone)
        {
            _currentStone += delta;
            text = _stoneText;
            current = _currentStone;
            target = _targetStone;
        }
        else
            throw new ArgumentException();
        
        StartCoroutine(ChangeText(text, current, target));
    }

    private void Start()
    {
        _targetWood = _houseBuilder.GetResourcesCount(Constants.Resources.Wood);
        _targetStone = _houseBuilder.GetResourcesCount(Constants.Resources.Stone);
        SetText(_woodText, _currentWood, _targetWood);
        SetText(_stoneText, _currentStone, _targetStone);
    }

    private IEnumerator ChangeText(TMP_Text text, int current, int target)
    {
        text.transform.DOScale(Vector3.one * _maxScale, _animationDelay);
        yield return new WaitForSeconds(_animationDelay);
        SetText(text, current, target);
        text.transform.DOScale(Vector3.one, _animationDelay);
    }

    private void SetText(TMP_Text text, int current, int target)
    {
        if (current < 0)
            current = 0;
        
        text.text = current + "/" + target;
        
        if(current >= target)
            text.color = Color.green;
        else 
            text.color = Color.white;
    }

    public void Disable()
    {
        transform.DOScale(Vector3.zero, _animationDelay);
    }
}
