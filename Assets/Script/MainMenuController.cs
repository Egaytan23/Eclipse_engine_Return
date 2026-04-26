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

    public void ShowHowToPlay()
    {
       mainMenuPanel.SetActive(false);
         howToPlayPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        howToPlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        //# is a preprocessor directive its like instructions to unity before the code runs
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // stops Play Mode
#else
    Application.Quit();                                // quits build
#endif
        PlayerPrefs.DeleteKey("SessionStarted");
    }
}

