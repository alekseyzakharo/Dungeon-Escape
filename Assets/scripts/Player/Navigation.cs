using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [HideInInspector]
    public enum States { idle, running, crouchIdle, crouchWalk };
    public States currentState;

    public static Vector3 Destination;

    NavMeshAgent agent;
    [Range(0, 100)]

    public float RunSpeed = 5;
    public float CrouchSpeed = 3;

    LayerMask Hidden;
    LayerMask NotHidden;

    private bool buttonDown;
    private float navDelaySlower = 0.95f;

    // Use this for initialization
    void Start()
    {
        currentState = States.idle;

        agent = GetComponent<NavMeshAgent>();

        Hidden = LayerMask.NameToLayer("Hidden");
        NotHidden = LayerMask.NameToLayer("Targets");

        Destination = transform.position;
        StartCoroutine("CheckForButtonDown", Globals.navigationDelayTime * navDelaySlower);
    }

    // Update is called once per frame
    void Update()
    {
        //new click on map
        if (Input.GetMouseButtonDown(0))
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
                    if (Globals.DistanceV3xz(transform.position, hit.point) >= Globals.CROUCHIDLE_EPSI)
                    {
                        StartCoroutine("SetNewDestination", hit.point);
                    }
                }
            }
        }

        //Update player into the correct state
        if(Globals.DistanceV3xz(Destination, transform.position) > Globals.CROUCHIDLE_EPSI)
        {
            if (buttonDown && Input.GetKey(KeyCode.Mouse0))
            {
                //crouch walk
                currentState = States.crouchWalk;
                gameObject.layer = Hidden;
            }
            else
            {
                //run
                currentState = States.running;
                gameObject.layer = NotHidden;
            }
        }
        else
        {
            if (buttonDown && Input.GetKey(KeyCode.Mouse0))
            {
                //crouch idle
                currentState = States.crouchIdle;
                gameObject.layer = Hidden;
            }
            else
            {
                //idle
                currentState = States.idle;
                gameObject.layer = NotHidden;
            }
        }
    }
          
    IEnumerator SetNewDestination(Vector3 destination)
    {
        yield return new WaitForSeconds(Globals.navigationDelayTime);
        SetDestination(destination);
    }

    private void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        Destination = destination;
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
}