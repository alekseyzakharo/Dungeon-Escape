using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOne : MonoBehaviour
{
    GameObject inGameMenu;

    private void Awake()
    {
        inGameMenu = GameObject.Find("In-GameMenu");
    }

    // Use this for initialization
    void Start()
    {

    }

    public void NextLevel()
    {
        DontDestroyOnLoad(inGameMenu);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
