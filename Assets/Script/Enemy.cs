using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public int damageToPlayer = 1;
    public float damageInterval = 1f;
    public float attackRange = 0.5f;
    public int maxHealth = 5;

    private int currentHealth;
    private bool canDamage = true;
    private bool isDead = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isDead && player != null)
        {
            FollowPlayer();
            CheckForAttack();
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Player", "Obstacle"));
            if (hit.collider == null || hit.collider.CompareTag("Player"))
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }

            if (direction.x < 0)
            {
                spriteRenderer.flipX = true; 
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            AttackPlayer();
        }
    }

    private void CheckForAttack()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (canDamage)
            {
                DealDamage();
            }
        }
    }

    private void DealDamage()
    {
        PlayerStats playerHealth = player.GetComponent<PlayerStats>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageToPlayer);
            StartCoroutine(DamageCooldown());
        }
    }

    private void AttackPlayer()
    {
        //animator.SetTrigger("Attack");

        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false; 
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageInterval);
        canDamage = true;
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        //animator.SetTrigger("Die");  
        //rb.velocity = Vector2.zero; 
        Destroy(gameObject); 
    }
}
