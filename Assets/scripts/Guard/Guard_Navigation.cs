using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Navigation : MonoBehaviour {
    

    public static bool patrol = true;
    public Transform startPos;
    public Transform endPos;


    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        startPos = transform;
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if(patrol && endPos != null)
        {
            if(Globals.CompareV3xz(transform.position, startPos.position))
            {
                agent.SetDestination(endPos.TransformPoint(Vector3.zero)); //global position of this child transform
            }
            else if(Globals.CompareV3xz(transform.position, endPos.position))
            {
                agent.SetDestination(startPos.TransformPoint(Vector3.zero));
            }
        }
	}

}
