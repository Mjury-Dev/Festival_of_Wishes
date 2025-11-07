using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Teleport : MonoBehaviour
{
    [Header("Teleport Destination")]
    [Tooltip("Assign the destination Transform (like another hole or portal).")]
    public Transform destination;

    [Header("Settings")]
    [Tooltip("Only teleport objects tagged as 'Player'.")]
    public bool requirePlayerTag = true;

    // Keeps track of the last teleport used
    private static Transform lastUsedTeleport = null;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destination == null) return;
        if (requirePlayerTag && !other.CompareTag("Player")) return;

        // Prevent teleporting from the same portal twice in a row
        if (lastUsedTeleport == transform) return;

        // Teleport player
        if (other.attachedRigidbody != null)
            other.attachedRigidbody.linearVelocity = Vector2.zero;

        other.transform.position = destination.position;

        // Mark this teleport as the last used one
        lastUsedTeleport = destination;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (requirePlayerTag && !other.CompareTag("Player")) return;

        // Reset once player fully leaves
        if (lastUsedTeleport == transform)
            lastUsedTeleport = null;
    }

    private void OnDrawGizmos()
    {
        if (destination == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, destination.position);
        Gizmos.DrawWireSphere(destination.position, 0.3f);
    }
}
