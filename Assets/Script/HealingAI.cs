using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAI : MonoBehaviour
{
    public Transform player; 
    public float healRange = 2.0f; 
    public float healAmount = 1.0f;  
    public float healCooldown = 3.0f; 
    public float followSpeed = 2.0f;  
    public float stopDistance = 1.5f;  

    private bool canHeal = true; 
    private SpriteRenderer spriteRenderer; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void Update()
    {
        FollowPlayer(); 

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);  
        if (distanceToPlayer <= healRange && canHeal) 
        {
            HealPlayer();  
        }
    }
    void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized; 

            if (Vector3.Distance(transform.position, player.position) > stopDistance)  
            {
                transform.position += direction * followSpeed * Time.deltaTime;  

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
    void HealPlayer()
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();  
        if (playerStats != null && playerStats.currentHealth < playerStats.maxHealth) 
        {
            int healAmount = Mathf.Min((int)this.healAmount, playerStats.maxHealth - playerStats.currentHealth);

            if (healAmount > 0)
            {
                playerStats.currentHealth += healAmount;
                playerStats.gameManager.UpdateHealthUI(playerStats.currentHealth); 
                Debug.Log($"Healed {healAmount} health to the player.");
            }

            StartCoroutine(ResetHealCooldown());
        }
    }
    private IEnumerator ResetHealCooldown()
    {
        canHeal = false; 
        yield return new WaitForSeconds(healCooldown); 
        canHeal = true; 
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
