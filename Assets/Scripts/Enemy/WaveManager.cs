using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// Is responsible to control, start and end enemies waves
public class WaveManager : MonoBehaviour
{
    int currentWave;

	[SerializeField] List<Wave> waves = new List<Wave>();
	[SerializeField] Transform leftSpawnPoint, rightSpawnPoint;

	List<Enemy> enemies = new List<Enemy>();
	int spawnCount;
	int deadEnemies;

	[Inject] HUD hud;
	[Inject] PowerUpManager powerUpManager;
	[Inject] EndScreen endScreen;
	[Inject] Player player;

	void Start()
	{
		StartNewWave();
	}

	// Finish the current wave when all enemies are defeateds
	void EndWave()
	{
		if (currentWave == waves.Count)
		{
			endScreen.EndGame(EndResult.Win);
		}
		else
		{
			powerUpManager.StartPowerUp();
			player.Stop();
		}
	}

	// Start a new wave
	public void StartNewWave()
	{
		StartCoroutine(StartNewWaveRoutine());
	}

	// Setup the wave infos and instantiate enemies
	IEnumerator StartNewWaveRoutine()
	{
		currentWave++;

		spawnCount = waves[currentWave].enemiesCount;
		deadEnemies = 0;

		hud.UpdateCurrentWaveText(currentWave);
		hud.UpdateRemainingEnemiesText(spawnCount);

		for (int i = 0; i < spawnCount; i++)
		{
			InstantiateEnemy();
			yield return new WaitForSeconds(Random.Range(2.5f, 3.5f));
		}

	}

	// Instantiate an enemy
	Enemy InstantiateEnemy()
	{
		EnemySpawnSettings settings = GetRandomEnemySpawnSettings();

		Enemy enemy = Instantiate(settings.enemyPrefab, settings.spawnPoint.position, Quaternion.identity);
		enemy.WaveManager = this;

		enemies.Add(enemy);

		return enemy;
	}

	// Return a random enemy settings 
	EnemySpawnSettings GetRandomEnemySpawnSettings()
	{
		EnemySpawnSettings settings;

		Wave wave = waves[currentWave - 1];
		settings.enemyPrefab = wave.enemiesPrefabs[Random.Range(0, wave.enemiesPrefabs.Count)];
		
		if (Random.value > .5f)
		{
			settings.spawnPoint = rightSpawnPoint;
		}
		else
		{
			settings.spawnPoint = leftSpawnPoint;
		}

		return settings;
	}

	// Control the end of wave and current enemies count
	public void OnEnemyDied(Enemy enemy)
	{
		if (enemies.Contains(enemy))
		{
			enemies.Remove(enemy);
			deadEnemies++;
		}

		hud.UpdateRemainingEnemiesText(spawnCount - deadEnemies);
		
		if (enemies.Count <= 0)
		{
			EndWave();
		}
	}

	// Move the enemy to a random start position
	public void ReplaceEnemy(Enemy enemy)
	{
		enemy.transform.position = Random.value > .5f ? leftSpawnPoint.position : rightSpawnPoint.position;
	}
}

// Contains the properties to spawn an enemy
public struct EnemySpawnSettings
{
	public Enemy enemyPrefab;
	public Transform spawnPoint;
}