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
		if(!Globals.CompareV3xz(Navigation.target, transform.position))
        {
            if (Navigation.RunCrouch)
            {
                animator.SetBool("movement", false);
                animator.SetBool("crouching", true);
            }
            else
            {
                animator.SetBool("crouching", false);
                animator.SetBool("movement", true);
            }
        }
        else
        {
            animator.SetBool("crouching", false);
            animator.SetBool("movement", false);
        }
    }


}
