using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [HideInInspector]
    public const float TRIGGERDISTANCE = 5f;

    public float radius;
    public int numOfPoints;
    [Range(0,100)]
    public float lineWidth;
    public Material lineMat;
    public GameObject triggerObj;
    public string triggerMethodName;
    [Range(0, 100)]
    public float leverAnimationSpeed;

    LineRenderer circle;
    Vector3[] points;

    private float ringHeight;
    public float highestPointBounce;
    public float lowestPointBounce;
    public float yIncrement;

    private bool pulled;
    //hard coded angles derived from best looking positions from camera angle
    private float stopLeverAngle = 160;
    Transform metalHandle;

    private void Awake()
    {
        circle = GetComponent<LineRenderer>();
    }

    // Use this for initialization
    void Start () {

        circle.material = lineMat;
        circle.widthMultiplier = lineWidth / 100;
        circle.positionCount = numOfPoints+1;

        ringHeight = lowestPointBounce;

        points = CalcualteCirclePoints(numOfPoints, radius);
        pulled = false;
        metalHandle = transform.Find("metal_handle");
    }
	
	// Update is called once per frame
	void Update () {
        if(!pulled)
        {
            if (ringHeight < lowestPointBounce || ringHeight > highestPointBounce)
            {
                yIncrement *= -1;
            }
            ringHeight += yIncrement;

            for(int i = 0 ; i < numOfPoints + 1 ; i++)
            {
                points[i].y = ringHeight;
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
            vec[iterator++] = new Vector3(x, lowestPointBounce, z) + transform.position;
        }
        return vec;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!pulled)
            {
                if (DistanceV3xz(GameObject.Find("Player").transform.position, transform.position) <= TRIGGERDISTANCE)
                {
                    //trigger lever animation
                    StartCoroutine("LeverAnimation", leverAnimationSpeed / 100);
                    triggerObj.gameObject.SendMessage(triggerMethodName, SendMessageOptions.DontRequireReceiver);
                    pulled = true;
                    circle.enabled = !circle.enabled; //turn off the circle animation
                }
            }
        }
    }

    IEnumerator LeverAnimation(float time)
    {
        //hard coded angle determined from 
        while (metalHandle.eulerAngles.z > stopLeverAngle)
        {
            yield return new WaitForSeconds(time);
            metalHandle.Rotate(new Vector3(0, 0, 1), -1);
            
        }
    }

    private float DistanceV3xz(Vector3 a, Vector3 b)
    {
        float x = a.x - b.x;
        float z = a.z - b.z;
        return Mathf.Sqrt((x * x) + (z * z));
    }

}
