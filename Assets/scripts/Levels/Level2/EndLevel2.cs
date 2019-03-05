using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel2 : MonoBehaviour {

    private GameObject LevelObj, player;

    void Awake()
    {
        LevelObj = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
    }
	
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.Equals(player))
        {
            LevelObj.SendMessage("NextLevel", SendMessageOptions.DontRequireReceiver);
        }
    }
}
