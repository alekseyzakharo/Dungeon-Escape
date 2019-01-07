﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Navigation : MonoBehaviour {
    
    public bool patrol = false;
    public Transform endPos;

    private new Guard_Animation animation;

    private Transform hips;

    CopyTransform start;
    CopyTransform end;

    Vector3 toPoint;

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        animation = gameObject.GetComponent<Guard_Animation>();
        start = new CopyTransform(transform);
        agent = GetComponent<NavMeshAgent>();
        hips = transform.Find("Hips");
        if (patrol)
        {
            end = new CopyTransform(endPos);
        }       
    }
	
	// Update is called once per frame
	void Update () {
        
		if(patrol && endPos != null)
        {
            if (Globals.CompareV3xz(transform.position, start.position()))
            {
                animation.Turn(); //activate turn animation
                //Transform hips = transform.Find("Hips");
                toPoint = end.position();
                //when direction is facing destination, stop turn and set destination
                if (Globals.IsFacingDirection(hips.forward, toPoint - hips.position))
                {
                    animation.TurnOff();
                    agent.SetDestination(end.position());
                }
            }
            else if(Globals.CompareV3xz(transform.position, endPos.position))
            {
                animation.Turn(); //activate turn animation
                //Transform hips = transform.Find("Hips");
                toPoint = start.position();
                if (Globals.IsFacingDirection(hips.forward, toPoint - hips.position))
                {
                    animation.TurnOff();
                    agent.SetDestination(start.position());
                }
            }
        }
	}

    public void Turn90Degrees()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }

}
