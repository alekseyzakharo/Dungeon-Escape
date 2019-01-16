using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{

    public float radius;
    public int numOfPoints;
    [Range(0,100)]
    public float lineWidth;
    public Material lineMat;

    LineRenderer circle;
    Vector3[] points;

    float y;
    public float highestPointBounce;
    public float lowestPointBounce;
    public float yIncrement;

    private bool pulled;
    private bool leverAnimationComplete;
    //hard coded angles derived from best looking positions from camera angle
    private float stopLeverAngle = 160;
    Transform metalHandle;

	// Use this for initialization
	void Start () {
        circle = GetComponent<LineRenderer>();
        circle.material = lineMat;
        circle.widthMultiplier = lineWidth / 100;
        circle.positionCount = numOfPoints+1;

        y = lowestPointBounce;

        points = CalcualteCirclePoints(numOfPoints, radius);

        pulled = false;
        leverAnimationComplete = false;
        metalHandle = transform.Find("metal_handle");

    }
	
	// Update is called once per frame
	void Update () {
        if(!pulled)
        {
            if (y < lowestPointBounce || y > highestPointBounce)
            {
                yIncrement *= -1;
            }
            y += yIncrement;

            for(int i = 0 ; i < numOfPoints + 1 ; i++)
            {
                points[i].y = y;
                circle.SetPosition(i, points[i]);
            }
            
        }
        else if(!leverAnimationComplete)
        {
            metalHandle.Rotate(transform.right, -1);
            if(metalHandle.localRotation.y == stopLeverAngle)
            {
                leverAnimationComplete = true;
            }
        }
	}

    private Vector3[] CalcualteCirclePoints(int numOfPoints, float rad)
    {
        Vector3 [] vec = new Vector3[numOfPoints+1];
        float theta = (2 * Mathf.PI) / numOfPoints;
        float x, z;
        int iterator = 0;
        for (float i = 0; i <= (2 * Mathf.PI) + theta ; i += theta)
        {
            x = rad * Mathf.Cos(i);
            z = rad * Mathf.Sin(i);
            vec[iterator++] = new Vector3(x, y, z) + transform.position;
        }
        return vec;
    }

    public void LeverPull()
    {
        pulled = true;
        //trigger door to glow
    }

    
}
