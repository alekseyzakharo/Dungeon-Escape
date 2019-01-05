using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {

    public static Vector3 target;
    public static bool RunCrouch;

    NavMeshAgent agent;
    public float RunSpeed = 5;
    public float CrouchSpeed = 3;

    

	// Use this for initialization
	void Start () {
        RunCrouch = false;
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
            RunCrouch = true;
        }
        if(Input.GetKey(KeyCode.Mouse0))
        {
            RunCrouch = true;
            agent.speed = CrouchSpeed;
        }
        else
        {
            RunCrouch = false;
            agent.speed = RunSpeed;
        }
    }

    

}
