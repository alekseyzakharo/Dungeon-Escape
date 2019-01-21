using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Navigation : MonoBehaviour {

    [HideInInspector]
    public enum States {idle, patrol, turning, sleeping, investigate, returnToPatrol};
    public States currentState;

    public float investigateTime;

    public float walkSpeed = 1.6f;
    public float runSpeed;

    public Transform endPos;

    //Patrolling Vars
    private Transform hips;
    private Vector3 currentDestination;
    private bool endDest = true;
    private Vector3 toVec;
    private Vector3 start;
    private Vector3 end;  

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
        switch(currentState)
        {
            case States.patrol:
                if (Globals.DistanceV3xz(transform.position, currentDestination) <= Globals.EPSI)
                {
                    if (endDest)
                        toVec = start - hips.position;
                    else
                        toVec = end - hips.position;
                    if (Vector3.Angle(hips.forward, toVec) > 180)
                    {
                        animation.TurnRight(); //activate turn right animation
                        currentState = States.turning;
                        break;
                    }
                    else
                    {
                        animation.TurnLeft(); //activate turn left animation
                        currentState = States.turning;
                        break;
                    }
                }
                break;
            case States.turning:
                if(IsFacingDirection(hips.forward, toVec))
                {
                    animation.TurnOffTurn();
                    if (endDest)
                        currentDestination = start;
                    else
                        currentDestination = end;
                    agent.SetDestination(currentDestination);
                    endDest = !endDest;
                    currentState = States.patrol;
                }
                break;
            case States.investigate:
                if (Globals.DistanceV3xz(transform.position, currentDestination) <= Globals.EPSI)
                {
                    animation.IdleInArea();
                    agent.speed = walkSpeed;
                    Invoke("GoBackToPatrol", investigateTime);
                }
                break;
            case States.returnToPatrol:
                if (Globals.DistanceV3xz(transform.position, currentDestination) <= Globals.EPSI)
                {
                    currentState = States.patrol;
                }
                break;
            default:
                break;
        }
	}

    public void InvestigateArea(Vector3 area)
    {
        CancelInvoke("GoBackToPatrol");
        animation.Attention();
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
        endDest = false;
        animation.AttentionOff();
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
