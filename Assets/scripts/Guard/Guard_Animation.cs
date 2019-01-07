using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_Animation : MonoBehaviour {

    public bool sleep = false;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        if (sleep)
        {
            sleep = true;
            anim.SetBool("sleep", true);
        }
        else
        {
            anim.SetBool("idle", true);
            //access another script attached to this game object to check a bool val in it
            if (gameObject.GetComponent<Guard_Navigation>().patrol)
            {
                anim.SetBool("walk", true);
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Turn()
    {
        anim.SetBool("turn", true);
    }

    public void TurnOff()
    {
        anim.SetBool("turn", false);
    }


}
