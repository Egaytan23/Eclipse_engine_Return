using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject howToPlayPanel;

    private KeyCode pauseKey = KeyCode.Escape;

    public string ReturnGameName = "Game_Teammate";
    public string ReturnMenuName = "Main Menu";


    public static bool isPaused;

    void Start()
    {
        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            isPaused = false;
        }
    }

    void Update()
    {
        if (pausePanel == null) return;

        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void ResumeGame() // Method to resume the game from a paused state
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void PauseGame() // Method to pause the game and show the pause menu
    {
        if (pausePanel == null) return;

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel() // Method to restart the current level
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(ReturnGameName);
    }

    public void LoadMainMenu() // Method to load the main menu scene
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(ReturnMenuName);
    }

    public void ShowHowToPlay() // Method to show the "How to Play" panel from the main menu
    {

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(true);
    }

    public void BackToMainMenuUI() // Method to return to the main menu from the "How to Play" panel
    {
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void LoadLoseScreen() // Method to load the lose screen when the player dies
    {
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Lose");
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
