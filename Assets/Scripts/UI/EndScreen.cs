using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Zenject;

public class EndScreen : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI titleText;
	[SerializeField] GameObject endPanel;
	[Inject] HUD hud;

    public void EndGame(EndResult result)
	{
		titleText.text = result == EndResult.GameOver ? "Game Over" : "You Win!";
		endPanel.SetActive(true);
	}

	public void PlayAgain()
	{
		Time.timeScale = 1;
		hud.RestartScene();
	}

	public void BackToMenu()
	{
		Time.timeScale = 1;
		hud.BackToMenu();
	}
}

public enum EndResult
{
    GameOver, Win
}