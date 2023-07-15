using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

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

	void Start()
	{
		StartNewWave();
	}

	void EndWave()
	{
		if (currentWave == waves.Count)
		{
			endScreen.EndGame(EndResult.Win);
		}
		else
		{
			powerUpManager.StartPowerUp();
		}
	}

	public void StartNewWave()
	{
		StartCoroutine(StartNewWaveRoutine());
	}

	IEnumerator StartNewWaveRoutine()
	{
		currentWave++;

		spawnCount = waves[currentWave].enemiesCount;
		deadEnemies = 0;

		hud.UpdateCurrentWave(currentWave);
		hud.UpdateRemainingEnemiesText(spawnCount);

		for (int i = 0; i < spawnCount; i++)
		{
			InstantiateEnemy();
			yield return new WaitForSeconds(Random.Range(2, 3));
		}

	}

	Enemy InstantiateEnemy()
	{
		EnemySpawnSettings settings = GetRandomEnemySpawnSettings();

		Enemy enemy = Instantiate(settings.enemyPrefab, settings.spawnPoint.position, Quaternion.identity);
		enemy.WaveManager = this;

		enemies.Add(enemy);

		return enemy;
	}

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

	public void ReplaceEnemy(Enemy enemy)
	{
		enemy.transform.position = Random.value > .5f ? leftSpawnPoint.position : rightSpawnPoint.position;
	}
}

public struct EnemySpawnSettings
{
	public Enemy enemyPrefab;
	public Transform spawnPoint;
}