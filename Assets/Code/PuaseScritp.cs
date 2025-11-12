using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuaseScritp : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public Button ButtonMenu;

    bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);

        Button btm = ButtonMenu.GetComponent<Button>();
        btm.onClick.AddListener(OnMenu);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                PauseGame();
                Debug.Log("This game is paused");
            }
            else
            {
                Resume();
                Debug.Log("This game has resume");
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1.0f;
    }
    public void PauseGame(){

        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void OnMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1.0f;
    }



}
