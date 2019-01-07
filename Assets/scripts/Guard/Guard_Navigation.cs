using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Navigation : MonoBehaviour {
    
    public bool patrol = false;
    public Transform endPos;

    private new Guard_Animation animation;

    CopyTransform start;
    CopyTransform end;

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        animation = gameObject.GetComponent<Guard_Animation>();
        start = new CopyTransform(transform);
        agent = GetComponent<NavMeshAgent>();

        if (patrol)
        {
            end = new CopyTransform(endPos);
        }       
    }
	
	// Update is called once per frame
	void Update () {
		if(patrol && endPos != null)
        {
            if(Globals.CompareV3xz(transform.position, start.position()))
            {
                //agent.SetDestination(end.position()); //global position of this child transform
                animation.Turn(); //turn on turn animation
                Vector3 toPoint = end.position();
                if(Globals.IsFacingDirection(gameObject.transform.forward, toPoint - transform.position))
                {
                    animation.StopTurn();
                    agent.SetDestination(end.position());
                }
            }
            else if(Globals.CompareV3xz(transform.position, endPos.position))
            {
                //agent.SetDestination(start.position());
                animation.Turn();
                Vector3 toPoint = start.position();
                if (Globals.IsFacingDirection(transform.forward, toPoint - transform.position))
                {
                    animation.StopTurn();
                    agent.SetDestination(start.position());
                }
            }
            //if(Globals.IsFacingDirection(transform.forward, ))
        }
	}

}
