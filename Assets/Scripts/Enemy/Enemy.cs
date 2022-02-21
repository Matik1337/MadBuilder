using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _animator.SetTrigger(Constants.Animations.Fight);
            
            player.Die();
        }
    }
}
