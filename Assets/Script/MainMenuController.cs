using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject howToPlayPanel;
    public GameObject mainMenuPanel;
    public void PlayGame()
    {
        SceneManager.LoadScene("Game_Teammate");
    }

    public void ShowHowToPlay() // Method to show the "How to Play" panel
    {
        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void BackToMainMenu() // Method to return to the main menu from the "How to Play" panel
    {
        howToPlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void OnApplicationQuit() // Method to clear PlayerPrefs when quitting the application
    {
        PlayerPrefs.DeleteKey("ItemsCollected");
        PlayerPrefs.DeleteKey("CurrentWave");
        PlayerPrefs.DeleteKey("CurrentWeapon");
        PlayerPrefs.Save();
    }
    public void QuitGame() // Method to quit the game and clear PlayerPrefs
    {
        Time.timeScale = 1f;

        PlayerPrefs.DeleteKey("ItemsCollected");
        PlayerPrefs.DeleteKey("CurrentWave");
        PlayerPrefs.DeleteKey("CurrentWeapon");
        PlayerPrefs.Save();
        // The following code is used to stop play mode in the Unity Editor, and quit the application in a built version of the game. This is necessary because Application.Quit() does not work in the Unity Editor, and will not stop play mode. By using conditional compilation, we can ensure that the correct behavior occurs in both cases.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}