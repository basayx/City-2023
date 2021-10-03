using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayKid : MonoBehaviour
{
    public Animator Animator;
    public Animator GraphicAnimator;
    public void Initialize(float delay)
    {
        Animator.SetTrigger("Walk");
        GraphicAnimator.SetTrigger("Walk");

        Destroy(gameObject, delay);
    }
}
