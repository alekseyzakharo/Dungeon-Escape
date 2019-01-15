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
        Time.timeScale = 0;
        transform.Find("Canvas").gameObject.SetActive(true);// = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
