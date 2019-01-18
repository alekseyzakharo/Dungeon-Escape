using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Navigation : MonoBehaviour {
    
    public enum States {idle, patrol, sleeping, investigate, returnToPatrol};
    public States currentState;

    public float investigateTime;

    public float walkSpeed = 1.6f;
    public float runSpeed;

    public Transform endPos;

    //Patrolling Vars
    private Transform hips;
    private Vector3 currentDestination;
    private bool endDest = true;
    private bool turning = false;
    private Vector3 start;
    private Vector3 end;

    //Investigate Vars
    private bool investigating;
    

    private new Guard_Animation animation;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        animation = GetComponent<Guard_Animation>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;

        start = transform.position;
        hips = transform.Find("Hips");

        if(currentState == States.patrol)
        {
            end = endPos.position;
            currentDestination = endPos.position;
            agent.SetDestination(currentDestination);
        }
    }
	
	// Update is called once per frame
	void Update () {
        
		if(currentState == States.patrol && endPos != null)
        {
            if(Globals.DistanceV3xz(transform.position, currentDestination) <= Globals.EPSI)
            {
                Vector3 toVec;
                if(endDest)
                    toVec = start - hips.position;
                else
                    toVec = end - hips.position;

                if (!turning && Vector3.Angle(hips.forward, toVec) > 180)
                {
                    animation.TurnRight(); //activate turn animation
                    turning = true;
                }
                else if (!turning)
                {
                    animation.TurnLeft(); //activate turn animation
                    turning = true;
                }
                if (IsFacingDirection(hips.forward, toVec))
                {
                    animation.TurnOffTurn();
                    if (endDest)
                        currentDestination = start;                    
                    else
                        currentDestination = end;
                    agent.SetDestination(currentDestination);
                    endDest = !endDest;
                    turning = false;
                }
            }
        }
        else if(currentState == States.investigate)
        {
            if (Globals.DistanceV3xz(transform.position, currentDestination) <= Globals.EPSI)
            {
                animation.IdleInArea();
                agent.speed = walkSpeed;
                Invoke("GoBackToPatrol", investigateTime);
            }
        }
        else if(currentState == States.returnToPatrol)
        {
            if (Globals.DistanceV3xz(transform.position, currentDestination) <= Globals.EPSI)
            {
                currentState = States.patrol;
            }
        }
	}

    public void InvestigateArea(Vector3 area)
    {
        animation.TurnOffTurn();
        currentState = States.investigate;
        currentDestination = area;
        agent.SetDestination(currentDestination);
        agent.speed = runSpeed;
        animation.Run();
    }

    private void GoBackToPatrol()
    {
        currentState = States.returnToPatrol;
        currentDestination = start;   
        agent.SetDestination(currentDestination);
        animation.Walk();
        turning = false;
        endDest = false;
    }


    //return true if the angle between the 2 vectors is less than EPSI_ANGLE
    private bool IsFacingDirection(Vector3 a, Vector3 b)
    {
        return Vector3.Angle(a, b) < Globals.EPSI_ANGLE;
    }

    public void TurnRight90Degrees()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }

    public void TurnLeft90Degrees()
    {
        transform.Rotate(new Vector3(0, -90, 0));
    }

}
