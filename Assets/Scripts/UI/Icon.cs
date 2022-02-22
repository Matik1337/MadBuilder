using System.Collections;
using UnityEngine;

public class Icon : MonoBehaviour
{
    private float delta = .3f;

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    private void OnDisable()
    {
        StopCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (enabled)
        {
            transform.localScale = Vector3.one + Vector3.one * Mathf.PingPong(Time.time, delta);
            yield return null;
        }
    }
}
