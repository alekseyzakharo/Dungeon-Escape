using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTwo : MonoBehaviour {

    public Transform playerWalkSpot;
    public GameObject MenuPrefab;
    
    private Navigation playerNav;
    private GameObject ceiling, inGameMenu;
    private ExitGate exitGate;

    private bool lever1, lever2;

    void Awake()
    {
        playerNav = GameObject.Find("Player").GetComponent<Navigation>();
        exitGate = GameObject.Find("Exit Gate").GetComponent<ExitGate>();
        ceiling = GameObject.Find("Map/ceiling");
        SetupInGameMenu();
    }

    // Use this for initialization
    void Start () {
        playerNav.SetDest(playerWalkSpot.position);
        lever1 = false;
        lever2 = false;
	}
	
    public void CeilingOff()
    {
        ceiling.SetActive(false);
    }

    public void AllowMovment()
    {
        playerNav.AllowMovement();
    }

    public void trig1()
    {
        lever1 = true;
        OpenGate();
    }

    public void trig2()
    {
        lever2 = true;
        OpenGate();
    }

    private void OpenGate()
    {
        if(lever1 && lever2)
        {
            exitGate.Trigger();
        }
    }

    private void SetupInGameMenu()
    {
        inGameMenu = GameObject.Find("In-GameMenu");
        if(inGameMenu == null)
        {
            Instantiate(MenuPrefab);
        }
    }

    public void NextLevel()
    {
        DontDestroyOnLoad(inGameMenu);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
