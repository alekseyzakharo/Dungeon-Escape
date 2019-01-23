using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOne : MonoBehaviour
{
    GameObject found, paused, mainSettings;

    private bool levelPause;

    // Use this for initialization
    void Start()
    {
        levelPause = false;
        found = GameObject.Find("Menu/Canvas/Found");
        paused = GameObject.Find("Menu/Canvas/Paused");
        mainSettings = GameObject.Find("Menu/Canvas/MainSettings");
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
        if(!levelPause)
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeTime();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        ResumeTime();
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
