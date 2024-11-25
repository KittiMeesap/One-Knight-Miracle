using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAI : MonoBehaviour
{
    public Transform player; 
    public float attackRange = 1.5f; 
    public LayerMask enemyLayer;  
    public int attackDamage = 2;  

    public float moveSpeed = 3.0f;  
    private bool isAttacking = false;  
    private SpriteRenderer spriteRenderer;  

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        FollowPlayer(); 

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        if (hitEnemies.Length > 0 && !isAttacking) 
        {
            AttackEnemy(hitEnemies[0].gameObject); 
        }
    }
    void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized; 
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > attackRange)
            {
                transform.position += direction * moveSpeed * Time.deltaTime; 

                if (player.position.x < transform.position.x)
                {
                    spriteRenderer.flipX = true; 
                }
                else if (player.position.x > transform.position.x)
                {
                    spriteRenderer.flipX = false; 
                }
            }
            else
            {
                if (player.position.x < transform.position.x)
                {
                    spriteRenderer.flipX = true;  
                }
                else if (player.position.x > transform.position.x)
                {
                    spriteRenderer.flipX = false; 
                }
            }
        }
    }
    void AttackEnemy(GameObject enemy)
    {
        isAttacking = true; 
        enemy.GetComponent<Enemy>().TakeDamage(attackDamage); 
        StartCoroutine(ResetAttackCooldown()); 
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(1f); 
        isAttacking = false; 
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
