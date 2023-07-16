using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using HemetToolkit;

// It manage the power ups and apply them on player 
public class PowerUpManager : MonoBehaviour
{
    [Inject] Player player;
    [Inject] WaveManager waveManager;

    [SerializeField] Transform panelsParent;
    [SerializeField] PowerUpPanel powerUpPanelPrefab;

    [SerializeField] List<PowerUp> powerUps = new List<PowerUp>();

    List<PowerUpPanel> panels = new List<PowerUpPanel>();

    // Allow power up choice to player
    public void StartPowerUp()
	{
        Time.timeScale = 0;
        panelsParent.gameObject.SetActive(true);
        ClearPanels();
        CreatePanels();
    }

    // Create random power up panels
    void CreatePanels()
	{
        List<PowerUp> possiblePowerUps = new List<PowerUp>(powerUps);
        if (player.Health.CurrentHealth == player.Health.MaxHealth) possiblePowerUps.RemoveAll(p => p.powerUpType == PowerUpType.Heal);
        ListToolkit.Shuffle(possiblePowerUps);

        for (int i = 0; i < 2; i++)
		{
            PowerUpPanel panel = Instantiate(powerUpPanelPrefab, panelsParent);
            panel.Setup(this, possiblePowerUps[i]);

            panels.Add(panel);
		}
	}

    // Destroy created power up panels
    public void ClearPanels()
	{
        foreach(PowerUpPanel panel in panels)
		{
            Destroy(panel.gameObject);
		}

        panels.Clear();
	}

    // Upgrade player skills
    public void ApplyPowerUp(PowerUp powerUp)
	{
		switch (powerUp.powerUpType)
		{
            case PowerUpType.Heal:
                player.Heal(1);
                break;
            case PowerUpType.IncreaseDamage:
                player.ProjectileDamage = player.ProjectileDamage + 1;
                break;
            case PowerUpType.IncreaseProjectileSpeed:
                player.ProjectileSpeed = player.ProjectileSpeed + 2.5f;
                break;
            case PowerUpType.IncreaseSpeed:
                player.MoveSpeed = player.MoveSpeed + 1;
                break;
            case PowerUpType.IncreaseRange:
                player.ProjectileLifetime = player.ProjectileLifetime + .25f;
                break;
		}

        Time.timeScale = 1;
        panelsParent.gameObject.SetActive(false);
		waveManager.StartNewWave();
        player.MoveToStartPosition();
	}
}

[System.Serializable]
public struct PowerUp
{
    public PowerUpType powerUpType;
    [TextArea] public string description;
}

public enum PowerUpType
{
    Heal, IncreaseSpeed, IncreaseProjectileSpeed, IncreaseDamage, IncreaseRange
}