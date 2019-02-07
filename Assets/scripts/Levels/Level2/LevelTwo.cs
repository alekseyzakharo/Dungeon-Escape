using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwo : MonoBehaviour {

    public Transform playerWalkSpot;

    Navigation playerNav;
    GameObject ceiling;

    void Awake()
    {
        playerNav = GameObject.Find("Player").GetComponent<Navigation>();
        ceiling = GameObject.Find("Map/ceiling");
    }

    // Use this for initialization
    void Start () {
        playerNav.SetDest(playerWalkSpot.position);
	}
	
    public void CeilingOff()
    {
        ceiling.SetActive(false);
    }

    public void AllowMovment()
    {
        playerNav.AllowMovement();
    }
}
