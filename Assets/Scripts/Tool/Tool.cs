using System;
using DG.Tweening;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] private Transform _axe;
    [SerializeField] private Transform _pick;
    [SerializeField] private float _maxScale;
    
    public const float EnablingDelay = .1f;

    private void Awake()
    {
        ChangeScale(_axe, Vector3.zero, 0f);
        ChangeScale(_pick, Vector3.zero, 0f);
    }

    public void Enable(string toolName, bool needEnable)
    {
        Vector3 targetScale;
        
        if(needEnable)
            targetScale = Vector3.one * _maxScale;
        else 
            targetScale = Vector3.zero;

        if (toolName == nameof(Constants.Tools.Axe))
            ChangeScale(_axe, targetScale, EnablingDelay);
        else if (toolName == nameof(Constants.Tools.Pick))
            ChangeScale(_pick, targetScale, EnablingDelay);
        else
            throw new ArgumentException();
    }

    private void ChangeScale(Transform tool, Vector3 targetValue, float delay)
    {
        tool.DOScale(targetValue, delay);
    }
}
