using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerCombat : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.4f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Attack Point")]
    [SerializeField] private Transform attackPoint; // Child transform
    [SerializeField] private float attackPointDistance = 0.5f; // How far in front of player

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = playerController?.GetAnimator() ?? GetComponent<Animator>();

        if (attackPoint == null)
        {
            attackPoint = transform.Find("AttackPoint");
            if (attackPoint == null)
                Debug.LogWarning("⚠️ PlayerCombat: AttackPoint not assigned or found. Please assign it in the Inspector.");
        }
    }

    private void Update()
    {
        if (playerController == null || animator == null) return;

        // Dynamically move AttackPoint in front of facing direction
        UpdateAttackPointPosition();

        if (Input.GetMouseButtonDown(0) && !playerController.isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private void UpdateAttackPointPosition()
    {
        if (attackPoint == null) return;

        Vector2 facing = playerController.GetLastMoveDirection();
        if (facing == Vector2.zero) return;

        // Position the attack point slightly ahead of player
        attackPoint.localPosition = facing.normalized * attackPointDistance;
    }

    private IEnumerator Attack()
    {
        playerController.isAttacking = true;

        Vector2 lastDir = playerController.GetLastMoveDirection();
        animator.SetFloat("moveX", lastDir.x);
        animator.SetFloat("moveY", lastDir.y);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.15f);
        DetectAndDamageEnemies();

        yield return new WaitForSeconds(attackDuration - 0.15f);
        playerController.isAttacking = false;
    }

    private void DetectAndDamageEnemies()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("⚠️ AttackPoint not assigned.");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy_Health health = enemy.GetComponent<Enemy_Health>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
