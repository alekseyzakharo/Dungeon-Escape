using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_Animation : MonoBehaviour {

    public bool sleep = false;
    public bool idle = false;

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
            idle = true;
            anim.SetBool("idle", true);
            if (Guard_Navigation.patrol)
            {
                anim.SetBool("walk", true);
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
