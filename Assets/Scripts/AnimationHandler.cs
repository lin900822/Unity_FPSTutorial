using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void PlayAnimation(Vector2 input)
    {
        var isMoving = input.magnitude >= 0.1f;
        
        if (isMoving)
        {
            _animator.SetFloat("xInput", input.x);
            _animator.SetFloat("yInput", input.y);

            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetFloat("xInput", 0);
            _animator.SetFloat("yInput", 0);

            _animator.SetBool("isWalking", false);
        }
    }
}
