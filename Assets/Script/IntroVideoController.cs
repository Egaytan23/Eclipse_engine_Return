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
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            videoPlayer.Stop();
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}
