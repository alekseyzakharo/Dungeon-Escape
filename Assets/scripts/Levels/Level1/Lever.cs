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
    public GameObject triggerObj;
    [Range(0, 100)]
    public float leverAnimationSpeed;

    LineRenderer circle;
    Vector3[] points;

    float y;
    public float highestPointBounce;
    public float lowestPointBounce;
    public float yIncrement;

    private bool pulled;
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

    //private void OnMouseOver()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (!pulled)
    //        {
    //            if (Globals.DistanceV3xz(GameObject.Find("Player").transform.position, transform.position) <= Globals.TRIGGER_DISTANCE)
    //            {
    //                //trigger lever animation
    //                StartCoroutine("LeverAnimation", leverAnimationSpeed / 100);
    //                triggerObj.gameObject.SendMessage("Trigger", SendMessageOptions.DontRequireReceiver);
    //                pulled = true;
    //                circle.enabled = !circle.enabled; //turn off the circle animation
    //            }
    //        }
    //    }
    //}

    IEnumerator LeverAnimation(float time)
    {
        //hard coded angle determined from 
        while (metalHandle.eulerAngles.z > stopLeverAngle)
        {
            yield return new WaitForSeconds(time);
            metalHandle.Rotate(new Vector3(0, 0, 1), -1);
            
        }
    }

}
