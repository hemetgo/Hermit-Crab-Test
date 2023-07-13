using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Collider2D groundCollider;
    protected float horizontalDirection;

    // Components
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Health health;

    protected virtual void Start()
    {
        // Assign the components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    protected virtual void Update()
    {
        
    }

    // Moves the character on the horizontal axis and control his animations
    protected virtual void Movement()
	{
        // Rigidbody velocity
        rb.velocity = new Vector2(horizontalDirection * moveSpeed, rb.velocity.y);

        // Sprite renderer
        if (rb.velocity.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (rb.velocity.x > 0) transform.localScale = new Vector3(1, 1, 1);
    }

    // Toggle character movement direction
    protected void SwitchDirection(Direction direction)
    {
        horizontalDirection = direction == Direction.Right ? 1 : -1;
    }

    // Decrease character health
    public virtual void TakeDamage(float damageValue)
	{
        health.DecreaseCurrentHealth(damageValue);

        if (health.IsDead())
		{
            Die();
		}
	}

    // Restore character health
    public virtual void Heal(float healValue)
	{
        health.IncreaseCurrentHealth(healValue);
	}

    // Responsible for the character die behaviour
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    // Return if character is on ground
    protected bool OnGround()
    {
        return Physics2D.Raycast(transform.position, -transform.up, .1f, LayerMask.GetMask("Ground"));
    }
}
