using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour {

    Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void AnimationStates(string _animation, bool state)
    {
        animator.SetBool(_animation, state);
    }
}
