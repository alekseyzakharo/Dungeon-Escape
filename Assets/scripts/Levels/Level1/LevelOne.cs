using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOne : MonoBehaviour
{
    GameObject inGameMenu;
    Navigation playerNav;

    private void Awake()
    {
        inGameMenu = GameObject.Find("In-GameMenu");
        playerNav = GameObject.Find("Player").GetComponent<Navigation>();
    }

    // Use this for initialization
    void Start()
    {
        playerNav.AllowMovement();
    }

    public void NextLevel()
    {
        DontDestroyOnLoad(inGameMenu);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
