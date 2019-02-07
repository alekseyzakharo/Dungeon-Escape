using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour {

    private GameObject player;
    private Animator camAnim;
    private bool playedAnimation;

    void Awake()
    {
        player = GameObject.Find("Player");
        camAnim = GameObject.Find("Main Camera").GetComponent<Animator>();
    }

    void Start () {
        playedAnimation = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!playedAnimation && other.gameObject.Equals(player))
        {
            camAnim.SetTrigger("camanim");
            playedAnimation = true;
        }
    }

}
