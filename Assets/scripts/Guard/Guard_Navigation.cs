using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Navigation : MonoBehaviour {

    [HideInInspector]
    public enum States {idle, patrol, turning, sleeping, investigate, returnToStartPosition};

    public States currentState;
    private States nextState;

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

    private GameObject fieldOfView;
    private IEnumerator returnToStart = null;

    private new Guard_Animation animation;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        animation = GetComponent<Guard_Animation>();
        agent = GetComponent<NavMeshAgent>();
        fieldOfView = transform.Find("Hips").GetComponent<Guard_FieldOfView>().gameObject;
        agent.speed = walkSpeed;

        start = transform.position;
        hips = transform.Find("Hips");

        if(currentState == States.patrol)
        {
            end = endPos.position;
            currentDestination = endPos.position;
            agent.SetDestination(currentDestination);
        }
        else if(currentState == States.sleeping)
        {
            fieldOfView.SetActive(false);
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
                    returnToStart = ReturnToInitialState(nextState);
                    StartCoroutine(returnToStart);
                }
                break;
            case States.returnToStartPosition:
                if (Globals.DistanceV3xz(transform.position, currentDestination) <= Globals.EPSI)
                {
                    switch(nextState)
                    {
                        case States.patrol:
                            currentState = nextState;
                            break;
                        case States.sleeping:
                            fieldOfView.SetActive(false);
                            animation.WalkOff();
                            animation.Sleep();
                            break;
                        default:
                            break;
                    }
                }
                break;
            default:
                break;
        }
	}

    public void InvestigateArea(Vector3 area, States nextState)
    {
        if(returnToStart != null)
            StopCoroutine(returnToStart);
        this.nextState = nextState;
        animation.Attention();
        switch (currentState)
        {
            case States.turning:
                animation.TurnOffTurn();
                break;
            case States.sleeping:
                fieldOfView.SetActive(true);
                break;
            default:
                break;
        }
        currentState = States.investigate;
        currentDestination = area;
        agent.SetDestination(currentDestination);
        agent.speed = runSpeed;
        animation.Run();
    }

    IEnumerator ReturnToInitialState(States state)
    {
        yield return new WaitForSeconds(investigateTime);
        currentState = States.returnToStartPosition;
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
