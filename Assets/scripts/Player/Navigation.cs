using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {

    [HideInInspector]
    public enum States { idle, running, crouching, crouchRunning};
    private States currentState;

    public static Vector3 Destination;

    NavMeshAgent agent;
    [Range(0,100)]

    public float RunSpeed = 5;
    public float CrouchSpeed = 3;

	// Use this for initialization
	void Start () {

        agent = GetComponent<NavMeshAgent>();
        Destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //new click on map
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(hit.point, path);

                if(path.status == NavMeshPathStatus.PathComplete)
                {
                    //check that distance of new click is greater than CrouchIdle_Epsi, to prevent floating around
                    if (Globals.DistanceV3xz(transform.position, hit.point) >= Globals.CROUCHIDLE_EPSI)
                    {
                        StartCoroutine("SetNewDestination", hit.point);
                    }
                }
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
}
