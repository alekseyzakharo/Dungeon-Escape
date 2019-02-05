using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Animation : MonoBehaviour {
    Animator animator;
    private Navigation nav;
    void Start () {
        animator = transform.Find("T-pose_Player").GetComponent<Animator>();
        nav = GetComponent<Navigation>();
        StartCoroutine("SetAnimation", Navigation.NAVDELAYTIME);
    }

    public void CrouchWalk()
    {
        animator.SetBool("run", false);
        animator.SetBool("crouch idle", false);
        animator.SetBool("crouch walk", true);
    }

    public void Run()
    {
        animator.SetBool("crouch walk", false);
        animator.SetBool("run", true);
    }

    public void CrouchIdle()
    {
        animator.SetBool("crouch idle", true);
    }

    public void Idle()
    {
        animator.SetBool("crouch idle", false);
        animator.SetBool("crouch walk", false);
        animator.SetBool("run", false);
    }

    IEnumerator SetAnimation(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            switch (nav.currentState)
            {
                case Navigation.States.idle:
                    Idle();
                    break;
                case Navigation.States.running:
                    Run();
                    break;
                case Navigation.States.crouchIdle:
                    CrouchIdle();
                    break;
                case Navigation.States.crouchWalk:
                    CrouchWalk();
                    break;
            }
        }
    }
}
