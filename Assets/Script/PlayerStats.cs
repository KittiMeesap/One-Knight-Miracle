using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    public int maxArmorSlots = 5;
    public int currentArmorSlots;

    public GameManager gameManager;

    public float baseSpeed = 5.0f;

    public int baseDamage = 1;
    private int currentDamage;

    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayer;

    private bool isImmortal = false;


    private bool isAttacking = false;
    private bool isHardAttacking = false;

    private bool hasSpeedBoost = false;
    private bool hasDamageBoost = false;
    private bool hasImmortality = false;

    private SpriteRenderer spriteRenderer;

    public Sprite normalSprite;
    public Sprite speedBoostSprite;
    public Sprite damageBoostSprite;
    public Sprite immortalitySprite;

    public Vector3 normalSize = new Vector3(1f, 1f, 1f);
    public Vector3 enlargedSize = new Vector3(1.5f, 1.5f, 1.5f);

    private GameObject currentPlayer;
    public float CurrentSpeed { get; private set; }

    void Start()
    {
        currentHealth = maxHealth;
        currentArmorSlots = maxArmorSlots;
        currentDamage = baseDamage;

        gameManager.UpdateHealthUI(currentHealth);
        gameManager.UpdateArmorUI(currentArmorSlots);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnRedPotionReceived()
    {
        transform.localScale = enlargedSize;

        StartCoroutine(ResetSizeAfterPotion());
    }

    private IEnumerator ResetSizeAfterPotion()
    {
        yield return new WaitForSeconds(5f); 
        transform.localScale = normalSize; 
    }

    public void LightAttack()
    {
        if (!isAttacking)
        {


            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<Enemy>().TakeDamage(currentDamage);
                }
            }

            StartCoroutine(ResetAttackState());
        }
    }

    public void HardAttack()
    {
        if (!isHardAttacking)
        {

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    enemy.GetComponent<Enemy>().TakeDamage(currentDamage * 2);
                }
            }

            StartCoroutine(ResetHardAttackState());
        }
    }

    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private IEnumerator ResetHardAttackState()
    {
        yield return new WaitForSeconds(1f);
        isHardAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isImmortal) return;

        if (currentArmorSlots > 0)
        {
            int damageToArmor = Mathf.Min(damage, currentArmorSlots);
            currentArmorSlots -= damageToArmor;
            damage -= damageToArmor;
        }

        if (damage > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        gameManager.UpdateHealthUI(currentHealth);
        gameManager.UpdateArmorUI(currentArmorSlots);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject);
    }

    public void AddArmor(string armorType)
    {
        if (armorType == "Light")
        {
            if (currentArmorSlots < maxArmorSlots)
            {
                currentArmorSlots += 1;
            }
        }
        else if (armorType == "Heavy")
        {
            int slotsAvailable = maxArmorSlots - currentArmorSlots;
            int armorToAdd = Mathf.Min(5, slotsAvailable);
            currentArmorSlots += armorToAdd;
        }

        gameManager.UpdateArmorUI(currentArmorSlots);

        Debug.Log($"Armor Type: {armorType}, Current Armor Slots: {currentArmorSlots}");
    }


    public void ActivateDamageBoost(float duration)
    {
        if (!hasDamageBoost)
        {
            Vector3 originalScale = transform.localScale;

            StartCoroutine(DamageBoostCoroutine(duration, originalScale));
            spriteRenderer.sprite = damageBoostSprite;
            hasDamageBoost = true;
        }
    }

    public void ActivateImmortality(float duration)
    {
        if (!hasImmortality)
        {
            Color originalColor = spriteRenderer.color;

            StartCoroutine(ImmortalityCoroutine(duration, originalColor));
            spriteRenderer.sprite = immortalitySprite;
            hasImmortality = true;
        }
    }

    private IEnumerator DamageBoostCoroutine(float duration, Vector3 originalScale)
    {
        int originalDamage = currentDamage;
        currentDamage *= 5;
        transform.localScale = originalScale * 2.0f;

        yield return new WaitForSeconds(duration);

        currentDamage = originalDamage;
        transform.localScale = originalScale;

        spriteRenderer.sprite = normalSprite;
        hasDamageBoost = false;
        Debug.Log("Damage Boost Ended.");
    }

    private IEnumerator ImmortalityCoroutine(float duration, Color originalColor)
    {
        spriteRenderer.color = Color.black;

        isImmortal = true;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor;

        isImmortal = false;

        spriteRenderer.sprite = normalSprite;
        hasImmortality = false;
        Debug.Log("Immortality Ended.");
    }

    public bool HasSpeedBoost()
    {
        return hasSpeedBoost;
    }

    public bool HasDamageBoost()
    {
        return hasDamageBoost;
    }

    public bool HasImmortality()
    {
        return hasImmortality;
    }
}
