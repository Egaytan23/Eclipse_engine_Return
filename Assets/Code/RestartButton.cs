using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
   
    void Start()
    {
        
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game_Teammate");
    }   
    void Update()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
