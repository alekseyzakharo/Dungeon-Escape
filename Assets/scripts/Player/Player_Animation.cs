using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Animation : MonoBehaviour {

    Animator animator;
	// Use this for initialization
	void Start () {
        animator = transform.Find("T-pose_Player").GetComponent<Animator>();
	}
	
	void Update () {
        float destinationDist = Globals.DistanceV3xz(Navigation.Destination, transform.position);
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(destinationDist > Globals.CROUCHIDLE_EPSI)
            {
                animator.SetBool("crouch idle", false);
                animator.SetBool("crouch walk", true);         
            }
            else
            {
                animator.SetBool("crouch idle", true);
            }
        }
        else
        {
            if(destinationDist > Globals.CROUCHIDLE_EPSI)
            {
                animator.SetBool("run", true);
            }
            else
            {
                animator.SetBool("crouch idle", false);
                animator.SetBool("crouch walk", false);
                animator.SetBool("run", false);
            }
        }


        //if(Navigation.inputPressed)
        //{
        //    if(Navigation.CrouchIdle)
        //    {
        //        animator.SetBool("crouch idle", true);
        //    }
        //    else
        //    {
        //        animator.SetBool("movement", false);
        //        animator.SetBool("crouching", true);
        //    }
        //}
        //else
        //{
        //    if(Globals.DistanceV3xz(Navigation.Destination, transform.position) >= Globals.DISTANCE_EPSI)
        //    {
        //        animator.SetBool("crouching", false);
        //        animator.SetBool("movement", true);
        //    }
        //    else
        //    {
        //        animator.SetBool("crouch idle", false);
        //        animator.SetBool("crouching", false);
        //        animator.SetBool("movement", false);
        //    }
        //}
    }


}
