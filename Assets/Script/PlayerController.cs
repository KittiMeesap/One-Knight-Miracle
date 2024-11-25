using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private Vector2 inputVector = new Vector2(0.0f, 0.0f);

    public SpriteRenderer spriteRenderer;
    public PlayerAnimation playerAnimation;

    public float baseSpeed = 5.0f;
    private float currentSpeed;

    public PlayerStats playerStats; 

    void Awake()
    {
        rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        rigidbody2D.angularDrag = 0.0f;
        rigidbody2D.gravityScale = 0.0f;

        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        playerStats = GetComponent<PlayerStats>();
        playerAnimation = GetComponent<PlayerAnimation>();

        currentSpeed = playerStats.baseSpeed;
    }

    void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetMouseButtonDown(0))
        {
            playerStats.LightAttack();
            playerAnimation.LightAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            playerStats.HardAttack();
            playerAnimation.HardAttack();
        }

        if (inputVector.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (inputVector.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + (inputVector * currentSpeed * Time.fixedDeltaTime));
    }

    public void ActivateSpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(duration));
    }

    private IEnumerator SpeedBoostCoroutine(float duration)
    {
        float originalSpeed = currentSpeed;
        currentSpeed *= 2.0f;
        yield return new WaitForSeconds(duration);
        currentSpeed = originalSpeed; 
    }

}

