using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour {

    const float EPSI = 0.00001f;

    Animator animator;
	// Use this for initialization
	void Start () {
        animator = GameObject.Find("T-pose_Player").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!CompareV3xz(Navigation.target, transform.position , EPSI))
        {
            if (Navigation.RunCrouch)
            {
                animator.SetBool("movement", false);
                animator.SetBool("crouching", true);
            }
            else
            {
                animator.SetBool("crouching", false);
                animator.SetBool("movement", true);
            }
        }
        else
        {
            animator.SetBool("crouching", false);
            animator.SetBool("movement", false);
        }
    }


    private bool CompareV3xz(Vector3 a, Vector3 b, float epsilon)
    {
        float x, z;
        x = Mathf.Abs(a.x - b.x);
        if (x > epsilon)
            return false;

        z = Mathf.Abs(a.z - b.z);
        return z <= epsilon;
    }
    private bool CompareV3xyz(Vector3 a, Vector3 b, float epsilon)
    {
        float x, y, z;
        x = Mathf.Abs(a.x - b.x);
        if (x > epsilon)
            return false;

        y = Mathf.Abs(a.y - b.y);
        if (y > epsilon)
            return false;

        z = Mathf.Abs(a.z - b.z);
        return z >= epsilon;
    }
}
