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
    private bool endDest = true;
    private Vector3 currentDestination, toVec, start, end;

    private Guard_FieldOfView fieldOfView;

    private IEnumerator returnToStart = null;

    Guard_Animation anim;
    NavMeshAgent agent;

    private void Awake()
    {
        anim = GetComponent<Guard_Animation>();
        agent = GetComponent<NavMeshAgent>();
        fieldOfView = transform.Find("Hips/FieldOfView").GetComponent<Guard_FieldOfView>();
        hips = transform.Find("Hips");
    }

    void Start ()
    {
        agent.speed = walkSpeed;
        start = transform.position;
        if (currentState == States.patrol)
        {
            end = endPos.position;
            currentDestination = endPos.position;
            agent.SetDestination(currentDestination);
        }
        else if (currentState == States.sleeping)
        {
            fieldOfView.enabled = false;
            //fieldOfView.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
        switch(currentState)
        {
            case States.patrol:
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
                if (Vector3.Angle(hips.forward, toVec) < EPSI_ANGLE)
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
                            fieldOfView.enabled = false;
                            //fieldOfView.SetActive(false);
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
                fieldOfView.enabled = true;
                //fieldOfView.SetActive(true);
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
        if(!agent.pathPending)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
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
