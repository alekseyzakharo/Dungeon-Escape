using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    public const float NAVDELAYTIME = 0.2f;
    public const float CROUCHIDLE_EPSI = 0.2f;

    [HideInInspector]
    public enum States { idle, running, crouchIdle, crouchWalk };
    [HideInInspector]
    public States currentState;

    public static Vector3 Destination;

    NavMeshAgent agent;

    LayerMask Crouch, Idle, Target;

    private GameObject LevelObj, NextLevelSquare;

    private bool buttonDown, canMove;
    private const float navDelaySlower = 0.95f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        LevelObj = GameObject.Find("Main Camera");
        NextLevelSquare = GameObject.Find("NextLevel");
    }

    // Use this for initialization
    void Start()
    {
        currentState = States.idle;
        Crouch = LayerMask.NameToLayer("Crouch");
        Idle = LayerMask.NameToLayer("Idle");
        Target = LayerMask.NameToLayer("Target");

        canMove = false;

        Destination = transform.position;
        StartCoroutine("CheckForButtonDown", NAVDELAYTIME * navDelaySlower);
    }

    // Update is called once per frame
    void Update()
    {
        //new click on map
        if (Input.GetMouseButton(0) && canMove)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(hit.point, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    //check that distance of new click is greater than CrouchIdle_Epsi, to prevent floating around
                    if (DistanceV3xz(transform.position, hit.point) >= CROUCHIDLE_EPSI)
                    {
                        StartCoroutine("SetNewDestination", hit.point);
                    }
                }
            }
        }

        //Update player into the correct state
        if(DistanceV3xz(Destination, transform.position) > CROUCHIDLE_EPSI)
        {
            if (buttonDown && Input.GetKey(KeyCode.Mouse0))
            {
                //crouch walk
                currentState = States.crouchWalk;
                gameObject.layer = Crouch;
            }
            else
            {
                //run
                currentState = States.running;
                gameObject.layer = Target;
            }
        }
        else
        {
            if (buttonDown && Input.GetKey(KeyCode.Mouse0))
            {
                //crouch idle
                currentState = States.crouchIdle;
                gameObject.layer = Crouch;
            }
            else
            {
                //idle
                currentState = States.idle;
                gameObject.layer = Idle;
            }
        }
    }
 
    IEnumerator SetNewDestination(Vector3 destination)
    {
        yield return new WaitForSeconds(NAVDELAYTIME);
        SetDestination(destination);
    }

    private void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        Destination = destination;
    }

    public void SetDest(Vector3 destination)
    {
        StartCoroutine("SetNewDestination", destination);
    }

    public void AllowMovement()
    {
        canMove = true;
    }

    //constantly check if the button is pressed
    IEnumerator CheckForButtonDown(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (Input.GetKey(KeyCode.Mouse0))
                buttonDown = true;
            else
                buttonDown = false;
        }
    }

    private float DistanceV3xz(Vector3 a, Vector3 b)
    {
        float x = a.x - b.x;
        float z = a.z - b.z;
        return Mathf.Sqrt((x * x) + (z * z));
    }


}