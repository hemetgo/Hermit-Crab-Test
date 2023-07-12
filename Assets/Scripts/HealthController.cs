using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float currentHealth;

	private void Start()
	{
		// Starts with current health maxed
		currentHealth = maxHealth;
	}

	// Decrease current health
	public void TakeDamage(float damageValue)
	{
        currentHealth -= damageValue;
	}

	// Restore current health
    public void Heal(float healValue)
	{
        currentHealth += healValue;

        if (currentHealth > maxHealth) currentHealth = maxHealth;
	}

	// Return true if is dead
    public bool IsDead()
	{
        return currentHealth <= 0;
	}
}
