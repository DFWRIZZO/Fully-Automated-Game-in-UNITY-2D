using System;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        PlayIdleAnimation();
    }

    private void PlayIdleAnimation()
    {
        animator.Play("Archer_Idle");

    }

    internal void ActivateShooting(bool v)
    {
        throw new NotImplementedException();
    }
}