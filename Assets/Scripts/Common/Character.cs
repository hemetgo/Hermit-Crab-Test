using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Collider2D groundCollider;
    protected float horizontalDirection;
    protected float defaultScale;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    // Components
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Health health;
    public Health Health { get => health; }

	private void Awake()
	{
        // Assign the components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

	protected virtual void Start()
    {
        // Save the default scale to flip horizontal with the fire point
        defaultScale = transform.localScale.x;
    }

    protected virtual void Update()
    {
        AcidFall();
    }

    // If fall on acid it dies
    protected virtual void AcidFall()
	{
        
    }

    // Moves the character on the horizontal axis and control his animations
    protected virtual void Movement()
	{
        // Rigidbody velocity
        rb.velocity = new Vector2(horizontalDirection * moveSpeed, rb.velocity.y);

        // Sprite renderer
        if (rb.velocity.x < 0) transform.localScale = new Vector3(-defaultScale, defaultScale, defaultScale);
        else if (rb.velocity.x > 0) transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
    }

    // Toggle character movement direction
    public void SwitchDirection(Direction direction)
    {
        horizontalDirection = direction == Direction.Right ? 1 : -1;
    }

    // Decrease character health
    public virtual void TakeDamage(int damageValue)
	{
        health.DecreaseCurrentHealth(damageValue);

        if (health.IsDead())
		{
            Die();
		}
	}

    // Restore character health
    public virtual void Heal(int healValue)
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
