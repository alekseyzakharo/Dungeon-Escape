using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {

    public static Vector3 target;
    public static bool RunCrouch;
    public static bool CrouchIdle;

    private bool ButtonHeld;
    private float HoldTime;

    NavMeshAgent agent;
    public float RunSpeed = 5;
    public float CrouchSpeed = 3;

	// Use this for initialization
	void Start () {
        RunCrouch = false;
        CrouchIdle = false;
        ButtonHeld = false;
        HoldTime = 0;
        agent = GetComponent<NavMeshAgent>();
        target = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                agent.SetDestination(hit.point);
                target = hit.point;
            }
            
        }
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(ButtonHeld)
            {
                Debug.Log(Time.time + " " + HoldTime);
                if(Time.time - HoldTime >= Globals.ELAPSED_TIME)
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
                }
            }
            else
            {
                ButtonHeld = true;
                HoldTime = Time.time;
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
            ButtonHeld = false;
            RunCrouch = false;
            CrouchIdle = false;
            agent.speed = RunSpeed;
        }
    }


}
