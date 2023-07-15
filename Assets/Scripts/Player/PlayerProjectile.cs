using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
	int damage;
	public int Damage { get => damage; }

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
