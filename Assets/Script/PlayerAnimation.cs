using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    private bool isIdle = false;
    private bool isWalking = false;
    private bool isAttacking = false;
    private bool isHardAttacking = false;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetIdle(bool value)
    {
        if (isIdle != value)
        {
            isIdle = value;
            animator.SetBool("isIdle", isIdle);
        }
    }

    public void SetWalking(bool value)
    {
        if (isWalking != value)
        {
            isWalking = value;
            animator.SetBool("isWalking", isWalking);
        }
    }

    public void LightAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("LightAttack");
            StartCoroutine(ResetAttackState(0.5f));
        }
    }

    public void HardAttack()
    {
        if (!isHardAttacking)
        {
            isHardAttacking = true;
            animator.SetTrigger("HardAttack");
            StartCoroutine(ResetHardAttackState(1f));
        }
    }

    private IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay); 
        isAttacking = false;
        animator.ResetTrigger("LightAttack"); 
    }

    private IEnumerator ResetHardAttackState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isHardAttacking = false;
        animator.ResetTrigger("HardAttack"); 
    }


    public void ResetAllAnimations()
    {
        SetIdle(false);
        SetWalking(false);
    }


    public void UpdatePlayerAppearance(Sprite playerSprite)
    {
        spriteRenderer.sprite = playerSprite;
    }

    public void UpdateAnimationDirection(Vector2 movement)
    {
        if (movement.x > 0) 
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < 0) 
        {
            spriteRenderer.flipX = true;
        }
    }
}
