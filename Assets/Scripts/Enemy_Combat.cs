using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange = 0.5f;
    public LayerMask playerLayer; // Check if player is in range.

    public void Attack()
    {
        Debug.Log("Attacking Player Now!");

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position, weaponRange, playerLayer);

        foreach (Collider2D player in hits)
        {
            player.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
    }

    // 👇 This draws a visual attack range circle in the Scene view
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red; // Choose any color you like
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}
