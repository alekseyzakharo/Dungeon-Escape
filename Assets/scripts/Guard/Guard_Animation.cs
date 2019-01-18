using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_Animation : MonoBehaviour {

    [Range(0, 100)]
    public float idleAnimSpeed;
    [Range(0, 100)]
    public float patrolAnimSpeed;
    [Range(0, 100)]
    public float runAnimSpeed;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        switch (gameObject.GetComponent<Guard_Navigation>().currentState)
        {
            case Guard_Navigation.States.sleeping:
                Sleep();
                break;
            case Guard_Navigation.States.patrol:
                Patrol();
                break;
            default:
                break;
        }
	}

    public void Sleep()
    {
        anim.SetBool("sleep", true);
    }

    public void Patrol()
    {
        anim.SetBool("walk", true);
        anim.speed = patrolAnimSpeed / 100;
    }
    public void Walk()
    {
        anim.SetBool("walk", true);
    }

    public void Run()
    {
        anim.SetBool("run", true);
        anim.speed = runAnimSpeed / 100;
    }

    public void IdleInArea()
    {
        anim.SetBool("run", false);
        anim.SetBool("walk", false);
        anim.speed = idleAnimSpeed / 100;
    }

    public void TurnRight()
    {
        anim.SetBool("turn right", true);
        //anim.SetTrigger("turnRight");
    }

    public void TurnLeft()
    {
        anim.SetBool("turn left", true);
        //anim.SetTrigger("turnLeft");
    }

    public void TurnOffTurn()
    {
        anim.SetBool("turn right", false);
        anim.SetBool("turn left", false);
    }


}
