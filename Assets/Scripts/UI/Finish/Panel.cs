using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class Panel : MonoBehaviour
{
    [SerializeField] private float _enablingDelay = 1.5f;
    
    private Button _button;
    private float _firstDelayScale = .6f;

    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
        _button.interactable = false;
    }

    private void Start()
    {
        Disable(0);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private IEnumerator AnimateEnabling(float maxScale, float delay)
    {
        float firstDelay = delay * _firstDelayScale;
        float secondDelay = delay - firstDelay;

        yield return new WaitForSeconds(_enablingDelay);
        
        transform.DOScale(Vector3.one * maxScale, firstDelay);

        yield return new WaitForSeconds(firstDelay);

        transform.DOScale(Vector3.one, secondDelay);

        yield return new WaitForSeconds(secondDelay);

        _button.interactable = true;
    }

    protected abstract void OnButtonClick();

    public void Enable(float maxScale, float delay)
    {
        StartCoroutine(AnimateEnabling(maxScale, delay));
    }

    protected void Disable(float delay)
    {
        transform.DOScale(Vector3.zero, delay);
    }
}
