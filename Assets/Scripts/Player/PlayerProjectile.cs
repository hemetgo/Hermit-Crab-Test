using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control the player projectile
public class PlayerProjectile : MonoBehaviour
{
	int damage;
	public int Damage { get => damage; }

	// Setup the movement from projectile
    public void Setup(Vector2 direction, float speed, int damage)
	{
		this.damage = damage;

		GetComponent<Rigidbody2D>().velocity = direction * speed;
		GetComponent<SpriteRenderer>().flipX = direction.x < 0;
	}

	public void OnHitted()
	{
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Wall"))
		{
			OnHitted();
		}
	}
}
