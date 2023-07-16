using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control the player projectile
public class PlayerProjectile : MonoBehaviour
{
	int damage;
	float range;
	public int Damage { get => damage; }

	Vector2 direction;
	public Vector2 Direction { get => direction; }

	Vector3 startPosition;
    private void Update()
    {
        if (Vector3.Distance(transform.position, startPosition) > range)
        {
			Destroy(gameObject);
        }
    }

    // Setup the movement from projectile
    public void Setup(Vector2 direction, float speed, int damage, float range)
	{
		this.damage = damage;
		this.range = range;
		this.direction = direction;

		startPosition = transform.position;

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
