using UnityEngine;
using UnityEngine.SceneManagement;

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
        Unsubscribe();
    }

    private void OnWon()
    {
        int currentLvl = SceneManager.GetActiveScene().buildIndex;
        
        if(currentLvl == 15)
            PlayerPrefs.SetInt(AmplitudeEvents.LastLevel, 1);
        else 
            PlayerPrefs.SetInt(AmplitudeEvents.LastLevel, currentLvl + 1);
        
        Unsubscribe();
        _winPanel.Enable(_maxScale, _animationDelay);
        _resourcesDispalyer.Disable();
    }

    private void OnLost()
    {
        Unsubscribe();
        _losePanel.Enable(_maxScale, _animationDelay);
        _resourcesDispalyer.Disable();
    }

    private void Unsubscribe()
    {
        _player.Won -= OnWon;
        _player.Lost -= OnLost;
    }
}
