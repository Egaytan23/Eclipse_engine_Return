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
        OnGame(); // Automatically load the game scene when this script starts
    }

    public void OnGame() // Method to load the game scene
    {

        SceneManager.LoadScene("Game");
    }

    public void OnMenu() // Method to load the main menu scene
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OnApplicationQuit() // Method to quit the application
    {
        Application.Quit();

        Debug.Log("Quitting the game");
    }

}