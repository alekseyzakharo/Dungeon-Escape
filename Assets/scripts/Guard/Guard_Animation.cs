﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_Animation : MonoBehaviour {

    public bool sleep = false;
    [Range(0, 100)]
    public float idleSpeed;

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
            anim.speed = idleSpeed / 100;
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

    public void TurnRight()
    {
        anim.SetBool("turn right", true);
    }

    public void TurnLeft()
    {
        anim.SetBool("turn left", true);
    }

    public void TurnOff()
    {
        anim.SetBool("turn right", false);
        anim.SetBool("turn left", false);
    }


}
