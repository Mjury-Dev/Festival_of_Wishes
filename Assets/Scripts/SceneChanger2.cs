using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SceneChanger2 : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Player entered scene changer. Loading scene: {sceneToLoad}");

            if (SceneFader.Instance != null)
                SceneFader.Instance.FadeToScene(sceneToLoad);
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }
}
