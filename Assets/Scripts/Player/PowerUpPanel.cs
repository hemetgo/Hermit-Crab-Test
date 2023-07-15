using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpPanel : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI descriptionText;

	PowerUp powerUp;
	PowerUpManager powerUpManager;

    public void Setup(PowerUpManager powerUpManager, PowerUp powerUp)
	{
		this.powerUpManager = powerUpManager;
		this.powerUp = powerUp;

		descriptionText.text = powerUp.description;
	}

	public void ApplyPowerUp()
	{
		powerUpManager.ApplyPowerUp(powerUp);
	}
}
