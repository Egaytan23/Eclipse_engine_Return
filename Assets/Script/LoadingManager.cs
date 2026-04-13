using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics.CodeAnalysis;

public class LoadLevelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      OnGame();
    }

    public void OnGame()
    {

        SceneManager.LoadScene("Game");
    }

    public void OnMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnApplicationQuit()
    {
        Application.Quit();

        Debug.Log("Quitting the game");
    }

}