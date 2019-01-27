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
    [Range(0, 100)]
    public float guardAttentionIconTime;

    [Range(0,100)]
    public float sleepAnimSpeed;

    GameObject ExclamationMark;
    GameObject SleepMark;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        ExclamationMark = transform.Find("Hips/Spine/Spine1/Spine2/Neck/Head/HeadTop_End/Exclamation Mark").gameObject;
        //entry animation is idle
        anim.speed = idleAnimSpeed / 100;
        switch (gameObject.GetComponent<Guard_Navigation>().currentState)
        {
            case Guard_Navigation.States.sleeping:
                Sleep();
                transform.Find("Hips").GetComponent<Guard_FieldOfView>().gameObject.SetActive(false);
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
    }

    public void TurnLeft()
    {
        anim.SetBool("turn left", true);
    }

    public void TurnOffTurn()
    {
        anim.SetBool("turn right", false);
        anim.SetBool("turn left", false);
    }

    public void Attention()
    {
        ExclamationMark.SetActive(true); 
    }

    public void AttentionOff()
    {
        ExclamationMark.SetActive(false);
    }

    //IEnumerator CheckForButtonDown(float time)
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(time);
    //        if (Input.GetKey(KeyCode.Mouse0))
    //            buttonDown = true;
    //        else
    //            buttonDown = false;
    //    }
    //}
}
