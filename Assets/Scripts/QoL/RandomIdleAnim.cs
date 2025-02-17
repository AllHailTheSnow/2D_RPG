using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAnim : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        //Get the animator component
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //if the animator component is null, return
        if (!anim) { return; }

        //Get the current state of the animator component
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        //Play the animation at a random time
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
