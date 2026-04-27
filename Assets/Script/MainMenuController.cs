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

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("ItemsCollected");
        PlayerPrefs.DeleteKey("CurrentWave");
        PlayerPrefs.DeleteKey("CurrentWeapon");
        PlayerPrefs.Save();
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;

        PlayerPrefs.DeleteKey("ItemsCollected");
        PlayerPrefs.DeleteKey("CurrentWave");
        PlayerPrefs.DeleteKey("CurrentWeapon");
        PlayerPrefs.Save();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}