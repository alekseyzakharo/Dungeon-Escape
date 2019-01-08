using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Animation : MonoBehaviour {

    Animator animator;
	// Use this for initialization
	void Start () {
        animator = GameObject.Find("T-pose_Player").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //!Globals.CompareV3xz(Navigation.target, transform.position))
        if (Vector3.Distance(Navigation.target, transform.position) >= Globals.DISTANCE_EPSI)
        {
            //this needs to be modified
            if (Navigation.RunCrouch)
            {
                
                animator.SetBool("movement", false);
                animator.SetBool("crouching", true);
            }
            else
            {
                if (Navigation.CrouchIdle)
                {
                    animator.SetBool("crouch idle", true);
                }
                else
                {
                    animator.SetBool("crouching", false);
                    animator.SetBool("movement", true);
                }
            }
        }
        else if(Navigation.CrouchIdle)
        {
            animator.SetBool("crouch idle", true);
        }
        else
        {
            animator.SetBool("crouch idle", false);
            animator.SetBool("crouching", false);
            animator.SetBool("movement", false);
        }
    }


}
