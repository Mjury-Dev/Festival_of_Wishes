using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class SceneChanger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad; // The name of the scene to load

    private void Reset()
    {
        // Automatically make sure collider is a trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only the player should trigger the scene change
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Player entered scene changer. Loading scene: {sceneToLoad}");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
