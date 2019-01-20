using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Animation : MonoBehaviour {

    
    private bool buttonDown;
    Animator animator;
	// Use this for initialization
	void Start () {
        animator = transform.Find("T-pose_Player").GetComponent<Animator>();
        buttonDown = false;

        //make the button hold time just a bit longer than the time required for the nav to set a new location
        StartCoroutine("CheckForButtonDown", Globals.navigationDelayTime * 0.95);
    }
	
	void Update () {
        if(Globals.DistanceV3xz(Navigation.Destination, transform.position) > Globals.CROUCHIDLE_EPSI)
        {
            if (buttonDown)
            {
                animator.SetBool("run", false);
                animator.SetBool("crouch idle", false);
                animator.SetBool("crouch walk", true);
            }
            else
            {
                animator.SetBool("crouch walk", false);
                animator.SetBool("run", true);
            }
        }
        else
        {
            if(buttonDown)
            {
                animator.SetBool("crouch idle", true);
            }
            else
            {
                animator.SetBool("crouch idle", false);
                animator.SetBool("crouch walk", false);
                animator.SetBool("run", false);
            }

        }


        //float destinationDist = Globals.DistanceV3xz(Navigation.Destination, transform.position);
        //if(Input.GetKey(KeyCode.Mouse0))
        //{
        //    if(destinationDist > Globals.CROUCHIDLE_EPSI)
        //    {
        //        animator.SetBool("crouch idle", false);
        //        animator.SetBool("crouch walk", true);         
        //    }
        //    else
        //    {
        //        animator.SetBool("crouch idle", true);
        //    }
        //}
        //else
        //{
        //    if(destinationDist > Globals.CROUCHIDLE_EPSI)
        //    {
        //        animator.SetBool("run", true);
        //    }
        //    else
        //    {
        //        animator.SetBool("crouch idle", false);
        //        animator.SetBool("crouch walk", false);
        //        animator.SetBool("run", false);
        //    }
        //}
    }

    //constantly check if the button is pressed
    IEnumerator CheckForButtonDown(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (Input.GetKey(KeyCode.Mouse0))
                buttonDown = true;
            else
                buttonDown = false;
        }
    }
}
