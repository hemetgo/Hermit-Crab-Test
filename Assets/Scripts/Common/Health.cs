using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    int currentHealth;

	public int MaxHealth { get => maxHealth; set => maxHealth = value; }
	public int CurrentHealth { get => currentHealth; }

	private void Start()
	{
		// Starts with current health maxed
		currentHealth = maxHealth;
	}

	// Decrease current health
	public void DecreaseCurrentHealth(int damageValue)
	{
        currentHealth -= damageValue;
	}

	// Restore current health
    public void IncreaseCurrentHealth(int healValue)
	{
        currentHealth += healValue;

        if (currentHealth > maxHealth) currentHealth = maxHealth;
	}

	// Restore all health
	public void RestoreAllHealth()
	{
		currentHealth = maxHealth;
	}

	// Return true if is dead
    public bool IsDead()
	{
        return currentHealth <= 0;
	}
}
