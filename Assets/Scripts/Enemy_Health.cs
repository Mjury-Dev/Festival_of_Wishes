using UnityEngine;
using System.Collections;

public class Enemy_Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;
    private int currentHealth;
    private bool isDead = false;

    [Header("Visual Feedback")]
    private SpriteRenderer spriteRenderer;
    public Color hurtColor = Color.white;
    public Color deathColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Optional Settings")]
    public Animator animator; // optional, if you still want animations
    public bool destroyOnDeath = true;

    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called when this enemy takes damage.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage! Remaining HP: {currentHealth}");

        // Visual flash feedback
        if (spriteRenderer != null)
            StartCoroutine(FlashColor(hurtColor));

        // Optional: animation trigger
        if (animator != null && animator.HasParameterOfType("Hurt", AnimatorControllerParameterType.Trigger))
            animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} died!");

        // Flash red for death
        if (spriteRenderer != null)
            StartCoroutine(FlashColor(deathColor, 0.3f));

        // Optional animation
        if (animator != null && animator.HasParameterOfType("Die", AnimatorControllerParameterType.Trigger))
            animator.SetTrigger("Die");

        // Disable collider so it no longer blocks
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Destroy after a short delay
        if (destroyOnDeath)
            Destroy(gameObject, 0.5f);
        else
            gameObject.SetActive(false);
    }

    /// <summary>
    /// Flashes the sprite with the given color for a short time.
    /// </summary>
    private IEnumerator FlashColor(Color flashColor, float duration = -1f)
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(duration > 0 ? duration : flashDuration);

        // Restore color if not dead
        if (!isDead)
            spriteRenderer.color = originalColor;
    }
}
