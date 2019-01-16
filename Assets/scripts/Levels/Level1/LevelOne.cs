using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOne : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public void EndLevel()
    {
        //freeze time
        FreezeTime();
        GameObject.Find("Menu").transform.Find("Canvas").gameObject.SetActive(true);
        //transform.Find("Canvas").gameObject.SetActive(true);// = true;
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
