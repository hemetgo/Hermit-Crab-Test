using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
	[SerializeField] float currentHealth;

	protected override void Start()
	{
		base.Start();
		SwitchDirection(Random.value < .5f ? Direction.Right : Direction.Left);
	}

	protected override void Update()
	{
		base.Update();

		Movement();
	}

	public void TakeDamage(float damage)
	{
		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Wall"))
		{
			SwitchDirection(horizontalDirection == 1 ? Direction.Left : Direction.Right);
		}
	}
}
