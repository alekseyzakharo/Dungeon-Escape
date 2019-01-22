using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGate : MonoBehaviour {


    public float maxHeight;
    [Range(0,100)]
    public float gateAnimSpeed;
    [Range(0, 100)]
    public float incAmount;

	// Use this for initialization
	void Start () {
        maxHeight += transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Trigger()
    {
        StartCoroutine("GateAnimation", gateAnimSpeed / 100);
    }

    IEnumerator GateAnimation(float time)
    {
        float inc = incAmount / 100;
        //hard coded angle determined from 
        while (transform.position.y < maxHeight)
        {
            yield return new WaitForSeconds(time);
            transform.Translate(0, inc, 0);
        }
    }
}
