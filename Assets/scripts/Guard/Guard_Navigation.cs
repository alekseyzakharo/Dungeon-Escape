using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard_Navigation : MonoBehaviour {
    
    public bool patrol = false;
    public Transform endPos;

    private new Guard_Animation animation;

    private Transform hips;

    CopyTransform start;
    CopyTransform end;

    Vector3 toPoint;

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        animation = gameObject.GetComponent<Guard_Animation>();
        start = new CopyTransform(transform);
        agent = GetComponent<NavMeshAgent>();
        hips = transform.Find("Hips");
        if (patrol)
        {
            end = new CopyTransform(endPos);
        }       
    }
	
	// Update is called once per frame
	void Update () {
        
		if(patrol && endPos != null)
        {
            if (Globals.DistanceV3xz(transform.position, start.position()) <= Globals.EPSI)
            {
                animation.Turn(); //activate turn animation
                //Transform hips = transform.Find("Hips");
                toPoint = end.position();
                //when direction is facing destination, stop turn and set destination
                if (IsFacingDirection(hips.forward, toPoint - hips.position))
                {
                    animation.TurnOff();
                    agent.SetDestination(end.position());
                }
            }
            else if(Globals.DistanceV3xz(transform.position, endPos.position) <= Globals.EPSI)
            {
                animation.Turn(); //activate turn animation
                //Transform hips = transform.Find("Hips");
                toPoint = start.position();
                if (IsFacingDirection(hips.forward, toPoint - hips.position))
                {
                    animation.TurnOff();
                    agent.SetDestination(start.position());
                }
            }
        }
	}

    //return true if the angle between the 2 vectors is less than EPSI_ANGLE
    private bool IsFacingDirection(Vector3 a, Vector3 b)
    {
        return Vector3.Angle(a, b) < Globals.EPSI_ANGLE;
    }

    public void Turn90Degrees()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }

}
