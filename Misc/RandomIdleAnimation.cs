using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAnimation : MonoBehaviour
{
    private Animator myAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Start()
    {
        if (myAnimator)
        {
            AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
            myAnimator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        }
    }
}
