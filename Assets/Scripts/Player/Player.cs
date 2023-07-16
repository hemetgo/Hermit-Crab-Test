using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HemetToolkit;
using Zenject;

public class Player : Character
{
    [Header("Player Settings")]
    [SerializeField] Vector3 startPosition = new Vector3(0, -5, 0);
    [SerializeField] float jumpForce;
    float invulnerableTime = .75f;
    bool isInvulnerable;

    [Header("Attack")]
    [SerializeField] int projectileDamage;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileLifetime;
    [SerializeField] Transform firePoint;
    [SerializeField] PlayerProjectile projectilePrefab;
    [SerializeField] GameObject muzzlePrefab;
    float isFiringCooldown = .3f;
    float  isFiringTimer;

    [Inject] PlayerHealthBar healthBar;
    [Inject] EndScreen endScreen;

    public int ProjectileDamage { get => projectileDamage; set => projectileDamage = value; }
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float ProjectileLifetime { get => projectileLifetime; set => projectileLifetime = value; }

	protected override void Update()
    {
        base.Update();

        PCInput();
        Movement();
        FiringReset();
    }

    // Moves the player on the horizontal axis and control his animations
    protected override void Movement()
	{
        // Stop and return if is firing
        if (isFiringTimer > 0)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        base.Movement();

        // Animations
        animator.SetBool("IsMoving", horizontalDirection != 0);
        animator.SetBool("OnGround", OnGround());
    }

    // Make the player jump
    public void Jump()
	{
        if (!OnGround()) return;

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
	}

    // Make the player fire
    IEnumerator FireRoutine()
    {
        if (OnGround() && isFiringTimer <= 0)
        {
            isFiringTimer = isFiringCooldown;
            animator.SetTrigger("Fire");

            yield return new WaitForSeconds(.1f);

            Vector2 direction = horizontalDirection < 0 ? Vector2.left : Vector2.right;

            PlayerProjectile projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.Setup(direction, projectileSpeed, projectileDamage);
            Destroy(Instantiate(muzzlePrefab, firePoint.position, Quaternion.identity), .2f);
            Destroy(projectile.gameObject, projectileLifetime);
        }
    }

    void FiringReset()
	{
        isFiringTimer -= Time.deltaTime;
	}

    #region Inputs
    void PCInput()
	{
        if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        if (Input.GetKeyDown(KeyCode.RightArrow)) SwitchDirection(Direction.Right);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SwitchDirection(Direction.Left);
        if (Input.GetKeyDown(KeyCode.Space)) Fire();
    }

    public void SwapDirection()
	{
        SwitchDirection(horizontalDirection == 1 ? Direction.Left : Direction.Right);
	}

    public void Fire()
	{
        StartCoroutine(FireRoutine());
	}
	#endregion

	public override void TakeDamage(int damageValue)
	{
		base.TakeDamage(damageValue);

        isInvulnerable = true;
        StartCoroutine(SpriteRendererToolkit.Blink(spriteRenderer, invulnerableTime, .05f));
        StartCoroutine(SetVulnerable());
        healthBar.UpdateBar();
    }

	public override void Heal(int healValue)
	{
		base.Heal(healValue);
        healthBar.UpdateBar();
    }

    protected override void AcidFall()
	{
        if (transform.position.y < -8)
        {
            TakeDamage(1);
            MoveToStartPosition();
        }
    }

    public void MoveToStartPosition()
	{
        transform.position = startPosition;
    }

    public void Stop()
    {
        horizontalDirection = 0;
    }

    protected override void Die()
	{
		base.Die();
        endScreen.EndGame(EndResult.GameOver);
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isInvulnerable)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            TakeDamage(enemy.Damage);
        }
        else if (collision.CompareTag("Saw") && !isInvulnerable)
		{
            Saw saw = collision.GetComponent<Saw>();
            
            TakeDamage(saw.Damage);
        }
    }

    IEnumerator SetVulnerable()
    {
        yield return new WaitForSeconds(invulnerableTime);
        isInvulnerable = false;
    }
}

public enum Direction
{
    Up, Down, Right, Left
}