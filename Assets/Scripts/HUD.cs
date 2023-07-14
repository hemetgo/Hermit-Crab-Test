using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI remainingEnemiesText;

    public void UpdateRemainingEnemiesText(int remainingEnemies)
	{
        remainingEnemiesText.text = remainingEnemies.ToString();
	}
}
