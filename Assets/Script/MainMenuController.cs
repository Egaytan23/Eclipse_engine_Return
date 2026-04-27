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

        //RESET WAVE + ITEMS HERE
        PlayerPrefs.DeleteKey("ItemsCollected");
        PlayerPrefs.DeleteKey("CurrentWave");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
