using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Navigation : MonoBehaviour {

    const float EPSI_ANGLE = 5.0f;

    [HideInInspector]
    public enum States {idle, patrol, turning, sleeping, investigate, returnToStartPosition};

    public States currentState;
    private States nextState;

    public float investigateTime, walkSpeed, runSpeed;
    public Transform endPos;

    //Patrolling Vars
    private Transform hips;
    private Vector3 currentDestination;
    private bool endDest = true;
    private Vector3 toVec;
    private Vector3 start;
    private Vector3 end;

    //private Guard_FieldOfView fieldOfView;

    public GameObject fieldOfView;

    private IEnumerator returnToStart = null;

    Guard_Animation anim;
    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Guard_Animation>();
        agent = GetComponent<NavMeshAgent>();

        //fieldOfView = transform.Find("Hips/FieldOfView").GetComponent<Guard_FieldOfView>();

        agent.speed = walkSpeed;

        start = transform.position;

        hips = transform.Find("Hips");

        if (currentState == States.patrol)
        {
            end = endPos.position;
            currentDestination = endPos.position;
            agent.SetDestination(currentDestination);
        }
        else if (currentState == States.sleeping)
        {
            //fieldOfView.enabled = false;
            fieldOfView.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
        switch(currentState)
        {
            case States.patrol:
                //if (Globals.DistanceV3xz(transform.position, currentDestination) <= Globals.EPSI)
                if(ReachedDestination())
                {
                    if (endDest)
                        toVec = start - hips.position;
                    else
                        toVec = end - hips.position;
                    if (Vector3.Angle(hips.forward, toVec) < 180)
                    {
                        anim.TurnRight(); //activate turn right animation
                        currentState = States.turning;
                        break;
                    }
                    else
                    {
                        anim.TurnLeft(); //activate turn left animation
                        currentState = States.turning;
                        break;
                    }
                }
                break;
            case States.turning:
                if (ReachedDestination())
                {
                    anim.TurnOffTurn();
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
                if (ReachedDestination())
                {
                    anim.IdleInArea();
                    agent.speed = walkSpeed;
                    returnToStart = ReturnToInitialState(nextState);
                    StartCoroutine(returnToStart);
                }
                break;
            case States.returnToStartPosition:
                if (ReachedDestination())
                {
                    switch(nextState)
                    {
                        case States.patrol:
                            
                            break;
                        case States.sleeping:
                            //fieldOfView.enabled = false;
                            fieldOfView.SetActive(false);
                            anim.WalkOff();
                            anim.Sleep();
                            break;
                        default:
                            break;
                    }
                    currentState = nextState;
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
        anim.Attention();
        switch (currentState)
        {
            case States.turning:
                anim.TurnOffTurn();
                break;
            case States.sleeping:
                //fieldOfView.enabled = true;
                fieldOfView.SetActive(true);
                break;
            default:
                break;
        }
        currentState = States.investigate;
        currentDestination = area;
        agent.speed = runSpeed;
        agent.SetDestination(currentDestination);
        anim.Run();
    }

    IEnumerator ReturnToInitialState(States state)
    {
        yield return new WaitForSeconds(investigateTime);
        currentState = States.returnToStartPosition;
        currentDestination = start;   
        agent.SetDestination(currentDestination);
        anim.Walk();
        endDest = false;
        anim.AttentionOff();
    }

    public bool ReachedDestination()
    {
        //if(agent.pathPending)
        //{
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        //}
        return false;
    }

    //return true if the angle between the 2 vectors is less than EPSI_ANGLE
    private bool IsFacingDirection(Vector3 a, Vector3 b)
    {
        return Vector3.Angle(a, b) < EPSI_ANGLE;
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
