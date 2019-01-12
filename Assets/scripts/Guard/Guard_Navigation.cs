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

    private bool turning;

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        animation = gameObject.GetComponent<Guard_Animation>();
        start = new CopyTransform(transform);
        agent = GetComponent<NavMeshAgent>();
        hips = transform.Find("Hips");
        if (patrol)
        {
            turning = false;
            end = new CopyTransform(endPos);
        }       
    }
	
	// Update is called once per frame
	void Update () {
        
		if(patrol && endPos != null)
        {
            if (Globals.DistanceV3xz(transform.position, start.position()) <= Globals.EPSI)
            {
                Vector3 toVec = end.position() - hips.position;
                //determine which direction to turn
                if (!turning && Vector3.Angle(hips.forward, toVec) > 180)
                {
                    animation.TurnRight(); //activate turn animation
                    turning = true;
                }
                else if(!turning)
                {
                    animation.TurnLeft(); //activate turn animation
                    turning = true;
                }
                
                //when direction is facing destination, stop turn and set destination
                if (IsFacingDirection(hips.forward, toVec))
                {
                    animation.TurnOff();
                    agent.SetDestination(end.position());
                    turning = false;
                }
            }
            else if(Globals.DistanceV3xz(transform.position, endPos.position) <= Globals.EPSI)
            {
                Vector3 toVec = start.position() - hips.position;
                //determine which direction to turn
                if (!turning && Vector3.Angle(hips.forward, toVec) > 180)
                {
                    animation.TurnRight(); //activate turn animation
                    turning = true;
                }
                else if(!turning)
                {
                    animation.TurnLeft(); //activate turn animation
                    turning = true;
                }

                if (IsFacingDirection(hips.forward, toVec))
                {
                    animation.TurnOff();
                    agent.SetDestination(start.position());
                    turning = false;
                }
            }
        }
	}

    

    //return true if the angle between the 2 vectors is less than EPSI_ANGLE
    private bool IsFacingDirection(Vector3 a, Vector3 b)
    {
        return Vector3.Angle(a, b) < Globals.EPSI_ANGLE;
    }

    public void TurnRight90Degrees()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }

    public void TurnLeft90Degrees()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }

}
