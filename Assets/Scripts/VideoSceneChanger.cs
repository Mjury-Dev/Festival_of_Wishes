using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneChanger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;        // Type the next scene name or assign via Inspector

    void Start()
    {
        // Subscribe to the event that triggers when the video finishes
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Auto goes to next scene
    }
}
