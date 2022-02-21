using System.Collections;
using UnityEngine;

public class PlayerAnimatorHolder : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMoveState(bool isRun)
    {
        _animator.SetBool(Constants.Animations.IsRun, isRun);
    }

    public void SetAnimation(string animationName, float delay)
    {
        StartCoroutine(SetTrigger(animationName, delay));
    }

    private IEnumerator SetTrigger(string animationName, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        _animator.SetTrigger(animationName);
    }
}
