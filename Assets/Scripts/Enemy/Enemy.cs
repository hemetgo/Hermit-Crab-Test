using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : Character
{
	[SerializeField] int damage;
	public int Damage { get => damage; }
	[SerializeField] Material hitMaterial;

	WaveManager waveManager;
	public WaveManager WaveManager { set => waveManager = value; }

	bool isTakingKnockback;
	bool isSwitchDirectionEnabled = true;

	protected override void Start()
	{
		base.Start();

		SwitchDirection(Random.value > .5f ? Direction.Right : Direction.Left);
	}

	protected override void Update()
	{
		base.Update();

		if (isTakingKnockback) return;

		Movement();
	}

	IEnumerator ApplyKnockback(Vector2 direction, float force, float knockbackTimeOut)
	{
		isTakingKnockback = true;
		rb.velocity = direction * force;

		yield return new WaitForSeconds(knockbackTimeOut);

		isTakingKnockback = false;
	}

	// Change the material
	IEnumerator ApplyDamageFeedback(float feedbackTimeOut)
	{
		Material defaultMaterial = spriteRenderer.material;
		spriteRenderer.material = hitMaterial;

		yield return new WaitForSeconds(feedbackTimeOut);

		spriteRenderer.material = defaultMaterial;
	}

	// When dies, notify the wave manager
	protected override void Die()
	{
		base.Die();

		waveManager.OnEnemyDied(this);
	}

	// When fall on acid, the enemy gets stronger
	protected override void AcidFall()
	{
		if (transform.position.y < -8)
		{
			waveManager.ReplaceEnemy(this);
			SwitchDirection(Random.value > .5f ? Direction.Right : Direction.Left);
			IncreasePower();
		}
	}

	// Makes the enemy stronger
	void IncreasePower()
	{
		moveSpeed++;
		defaultScale += .15f;
		health.RestoreAllHealth();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Take damage from projectile	
		if (collision.CompareTag("PlayerProjectile"))
		{
			PlayerProjectile projectile = collision.GetComponent<PlayerProjectile>();
			projectile.OnHitted();

			TakeDamage(projectile.Damage);
			StartCoroutine(ApplyDamageFeedback(.15f));

			Vector2 knockbackDirection = transform.position.x < collision.transform.position.x ? Vector2.left : Vector2.right;
			knockbackDirection.y = 1;
			StartCoroutine(ApplyKnockback(knockbackDirection, 5, .15f));
		}
	}

	void EnableSwitchDirectionEnable()
    {
		isSwitchDirectionEnabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
		// Switches enemy movement direction on collide with walls
		if (collision.CompareTag("Wall") && isSwitchDirectionEnabled)
		{
			SwitchDirection(horizontalDirection == 1 ? Direction.Left : Direction.Right);
			isSwitchDirectionEnabled = false;
			Invoke(nameof(EnableSwitchDirectionEnable), .1f);
		}
	}
}
