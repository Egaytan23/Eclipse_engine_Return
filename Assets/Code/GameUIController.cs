using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
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

    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void PauseGame()
    {
        if (pausePanel == null) return;

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(ReturnGameName);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(ReturnMenuName);
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
    }
}
