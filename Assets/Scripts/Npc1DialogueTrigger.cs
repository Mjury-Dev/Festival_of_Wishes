using UnityEngine;

public class Npc1DialogueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel; // drag your dialogue UI here

    private void Start()
    {
        // Make sure dialogue is hidden at the start
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Show dialogue when player touches Grandma
        if (collision.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true);
        }
    }
}
