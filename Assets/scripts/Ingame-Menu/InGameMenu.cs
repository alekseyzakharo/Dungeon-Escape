using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {

    public GameObject found, paused, mainSettings;
    private bool levelPause;

    private void Awake()
    {
        //found = GameObject.Find("In-GameMenu/Canvas/Found");
        //paused = GameObject.Find("In-GameMenu/Canvas/Paused");
        //mainSettings = GameObject.Find("In-GameMenu/Canvas/MainSettings");
    }

    // Use this for initialization
    void Start ()
    {
        levelPause = false;
    }
	
    public void EndLevel()
    {
        //freeze time
        FreezeTime();
        found.SetActive(true);
        mainSettings.SetActive(true);
    }

    public void Pause()
    {
        if (!levelPause)
        {
            FreezeTime();
            paused.SetActive(true);
            mainSettings.SetActive(true);
            levelPause = true;
        }
        else
        {
            paused.SetActive(false);
            mainSettings.SetActive(false);
            ResumeTime();
            levelPause = false;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResumeTime();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        ResumeTime();
        Destroy(GameObject.Find("In-GameMenu"));
    }

    private void FreezeTime()
    {
        Time.timeScale = 0;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1;
    }

}
