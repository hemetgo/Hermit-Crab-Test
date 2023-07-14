using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WaveManager : MonoBehaviour
{
    int remainingEnemies;
	public int RemainingEnemies { get => remainingEnemies; }

	[SerializeField] List<Enemy> enemiesPrefabs = new List<Enemy>();
	[SerializeField] Transform leftSpawnPoint, rightSpawnPoint;

	[Inject] HUD hud;

	void Start()
	{
		StartCoroutine(StartWave());
	}

	IEnumerator StartWave()
	{
		int spawnCount = Random.Range(5, 10);
		remainingEnemies = spawnCount;
		hud.UpdateRemainingEnemiesText(remainingEnemies);

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
		enemy.SwitchDirection(settings.direction);
		enemy.WaveManager = this;

		return enemy;
	}

	EnemySpawnSettings GetRandomEnemySpawnSettings()
	{
		EnemySpawnSettings settings;

		settings.enemyPrefab = enemiesPrefabs[Random.Range(0, enemiesPrefabs.Count)];
		
		if (Random.value > .5f)
		{
			settings.spawnPoint = rightSpawnPoint;
			settings.direction = Direction.Left;
		}
		else
		{
			settings.spawnPoint = leftSpawnPoint;
			settings.direction = Direction.Right;
		}

		return settings;
	}

	public void OnEnemyDied()
	{
		remainingEnemies--;
		hud.UpdateRemainingEnemiesText(remainingEnemies);
	}
}

public struct EnemySpawnSettings
{
	public Enemy enemyPrefab;
	public Transform spawnPoint;
	public Direction direction;
}