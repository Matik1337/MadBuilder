using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _animationDelay;
    [SerializeField] private float _maxScale;

    private WinPanel _winPanel;
    private LosePanel _losePanel;
    private ResourcesDispalyer _resourcesDispalyer;

    private void Awake()
    {
        _winPanel = GetComponentInChildren<WinPanel>();
        _losePanel = GetComponentInChildren<LosePanel>();
        _resourcesDispalyer = GetComponentInChildren<ResourcesDispalyer>();
    }

    private void OnEnable()
    {
        _player.Won += OnWon;
        _player.Lost += OnLost;
    }

    private void OnDisable()
    {
        _player.Won -= OnWon;
        _player.Lost -= OnLost;
    }

    private void OnWon()
    {
        _winPanel.Enable(_maxScale, _animationDelay);
        _resourcesDispalyer.Disable();
    }

    private void OnLost()
    {
        _losePanel.Enable(_maxScale, _animationDelay);
        _resourcesDispalyer.Disable();
    }
}
