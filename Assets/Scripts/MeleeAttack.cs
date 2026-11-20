using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.7f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float cooldown = 0.45f;
    [SerializeField] private float hitDelay = 0.15f;

    private Animator animator;
    private PlayerController controller;
    private float nextAttackTime;
    private bool useSecondAttack;

    void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!controller.canMove || Time.time < nextAttackTime) return;

        Mouse mouse = Mouse.current;
        if (mouse == null || !mouse.leftButton.wasPressedThisFrame) return;

        nextAttackTime = Time.time + cooldown;
        animator.SetTrigger(useSecondAttack ? "Attack2" : "Attack1");
        useSecondAttack = !useSecondAttack;
        StartCoroutine(DealDamage());
    }

    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(hitDelay);

        Vector3 offset = attackPoint.position - transform.position;
        offset.x = Mathf.Abs(offset.x) * controller.Facing;
        Vector2 center = transform.position + offset;

        HashSet<Enemy> alreadyHit = new HashSet<Enemy>();
        foreach (Collider2D hit in Physics2D.OverlapCircleAll(center, attackRadius))
        {
            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy != null && alreadyHit.Add(enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}