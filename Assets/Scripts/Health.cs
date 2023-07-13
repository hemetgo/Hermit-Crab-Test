using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float currentHealth;

	private void Start()
	{
		// Starts with current health maxed
		currentHealth = maxHealth;
	}

	// Decrease current health
	public void DecreaseCurrentHealth(float damageValue)
	{
        currentHealth -= damageValue;
	}

	// Restore current health
    public void IncreaseCurrentHealth(float healValue)
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
