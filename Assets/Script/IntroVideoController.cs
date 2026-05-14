using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "MainMenu";

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd; // Subscribe to the video end event
    }

    void OnVideoEnd(VideoPlayer vp) // Method called when the video finishes playing
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    // Update is called once per frame
    void Update()
    { // Check for any key press to skip the video
        if (Input.anyKeyDown)
        {
            videoPlayer.Stop();
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}
