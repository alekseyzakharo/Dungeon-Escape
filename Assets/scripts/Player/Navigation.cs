using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {

    public static Vector3 target;
    public static bool RunCrouch;
    public static bool CrouchIdle;

    private float HoldTime;

    NavMeshAgent agent;
    public float RunSpeed = 5;
    public float CrouchSpeed = 3;

	// Use this for initialization
	void Start () {
        RunCrouch = false;
        CrouchIdle = false;
        HoldTime = 0;
        agent = GetComponent<NavMeshAgent>();
        target = transform.position;
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
                //check that distance of new click is greater than CrouchIdle_Epsi, to prevent floating around
                if(Vector3.Distance(transform.position, hit.point) >= Globals.CROUCHIDLE_EPSI)
                {
                    agent.SetDestination(hit.point);
                    target = hit.point;
                }
            }
            
        }
        if(Input.GetKey(KeyCode.Mouse0))
        {

            if (Vector3.Distance(transform.position, agent.destination) <= Globals.CROUCHIDLE_EPSI)
            {
                CrouchIdle = true;
            }
            else
            {
                RunCrouch = true;
                agent.speed = CrouchSpeed;
            }

            ////check if your location is near your last destination, if true you are standing still
            //if (Vector3.Distance(transform.position, agent.destination) <= Globals.CROUCHIDLE_EPSI)
            //{
            //    CrouchIdle = true;
            //}
            //else
            //{
            //    RunCrouch = true;
            //    agent.speed = CrouchSpeed;
            //}
        }
        else
        {
            RunCrouch = false;
            CrouchIdle = false;
            agent.speed = RunSpeed;
        }
    }
}
