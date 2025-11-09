using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (fadeImage != null)
            fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reconnect fadeImage if it got destroyed in new scene
        if (fadeImage == null)
        {
            fadeImage = FindObjectOfType<Image>();
            if (fadeImage != null)
                fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1);
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndSwitchScenes(sceneName));
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        // Fade out
        yield return StartCoroutine(Fade(0, 1));

        // Load new scene
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Wait a frame so UI can rebuild
        yield return null;

        // Reconnect fadeImage (in case it was destroyed)
        if (fadeImage == null)
        {
            fadeImage = FindObjectOfType<Image>();
        }

        // Fade back in
        yield return StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeImage == null)
            yield break;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, normalized);

            if (fadeImage != null)
                fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);

            yield return null;
        }
    }
}
