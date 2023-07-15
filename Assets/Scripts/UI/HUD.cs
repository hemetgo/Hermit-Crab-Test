using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI remainingEnemiesText;
    [SerializeField] TextMeshProUGUI currentWaveText;

    [SerializeField] GameObject pauseScreen;

    public void UpdateRemainingEnemiesText(int remainingEnemies)
	{
        remainingEnemiesText.text = remainingEnemies.ToString();
	}

    public void UpdateCurrentWave(int currentWave)
	{
        currentWaveText.text = "WAVE " + currentWave;
	}

    public void Pause()
	{
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

	public void Resume()
	{
		pauseScreen.SetActive(false);
		Time.timeScale = 1;
	}

	public void RestartScene()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void BackToMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("Menu");
	}
}
