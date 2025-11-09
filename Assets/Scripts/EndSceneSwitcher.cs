using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndSceneSwitcher : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Subscribe to the event that triggers when the video finishes
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("MainMenu"); //Auto goes to the main menu
    }
}
