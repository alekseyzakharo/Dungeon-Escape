using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_Animation : MonoBehaviour {

    [Range(0, 100)]
    public float idleAnimSpeed;
    [Range(0, 100)]
    public float patrolAnimSpeed;
    [Range(0, 100)]
    public float runAnimSpeed;

    [Range(0,100)]
    public float sleepAnimSpeed;

    GameObject ExclamationMark;
    GameObject SleepMark;

    Animator anim;

    public LayerMask obstacleMask;
    public LayerMask targetMask;
    public float lineWidth;
    public Material lineMat;
    public int numOfPoints;
    public float heightFromFloor;
    public float radius;
    LineRenderer circle;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        circle = GetComponent<LineRenderer>();
        ExclamationMark = transform.Find("Hips/Spine/Spine1/Spine2/Neck/Head/HeadTop_End/Exclamation Mark").gameObject;
    }

    // Use this for initialization
    void Start ()
    {
        circle.material = lineMat;
        circle.widthMultiplier = lineWidth / 100;
        circle.positionCount = numOfPoints + 1;

        //entry animation is idle
        anim.speed = idleAnimSpeed / 100;

        switch (gameObject.GetComponent<Guard_Navigation>().currentState)
        {
            case Guard_Navigation.States.sleeping:
                Sleep();
                break;
            case Guard_Navigation.States.patrol:
                Patrol();
                break;
            default:
                break;
        }
    }

    public void Sleep()
    {
        CalcualteCirclePoints(numOfPoints);
        circle.enabled = true;
        anim.SetBool("sleep", true);
        StartCoroutine(ListenForEnemy(0.2f));
    }

    public void WakeUp()
    {
        circle.enabled = false;
        anim.SetBool("sleep", false);
    }

    public void Patrol()
    {
        anim.SetBool("walk", true);
        anim.speed = patrolAnimSpeed / 100;
    }
    public void Walk()
    {
        anim.SetBool("walk", true);
    }

    public void WalkOff()
    {
        anim.SetBool("walk", false);
    }

    public void Run()
    {
        anim.SetBool("run", true);
        anim.speed = runAnimSpeed / 100;
    }

    public void IdleInArea()
    {
        anim.SetBool("run", false);
        anim.SetBool("walk", false);
        anim.speed = idleAnimSpeed / 100;
    }

    public void TurnRight()
    {
        anim.SetBool("turn right", true);
    }

    public void TurnLeft()
    {
        anim.SetBool("turn left", true);
    }

    public void TurnOffTurn()
    {
        anim.SetBool("turn right", false);
        anim.SetBool("turn left", false);
    }

    public void Attention()
    {
        ExclamationMark.SetActive(true); 
    }

    public void AttentionOff()
    {
        ExclamationMark.SetActive(false);
    }

    private void CalcualteCirclePoints(int numOfPoints)
    {
        float theta = (2 * Mathf.PI) / numOfPoints;
        for (int i = 0; i < numOfPoints + 1; i++)
        {
            float angle = theta * i;
            Vector3 direction = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            RaycastHit hit;
            Vector3 point;
            if (Physics.Raycast(transform.position, direction, out hit, radius, obstacleMask))
                point = hit.point;
            else
                point = transform.position + direction * radius;

            point.y = heightFromFloor;
            circle.SetPosition(i, point);
        }
    }

    IEnumerator ListenForEnemy(float time)
    {
        bool run = true;
        while (run)
        {
            yield return new WaitForSeconds(time);
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, radius, targetMask);
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 direction = (target.position - transform.position).normalized;
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, direction, distanceToTarget, obstacleMask))
                {
                    WakeUp();
                    Vector3 loc = target.position;
                    yield return new WaitForSeconds(2f);
                    transform.GetComponent<Guard_Navigation>().InvestigateArea(loc, Guard_Navigation.States.sleeping);
                    run = false;
                }
            }
        }
    }

    
}
