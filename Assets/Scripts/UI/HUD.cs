using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.SceneManagement;

// Show infos on canvas and contains the pause ui functions
public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI remainingEnemiesText;
    [SerializeField] TextMeshProUGUI currentWaveText;

    [SerializeField] GameObject pauseScreen;

	[Inject] PauseManager pauseManager;

    public void UpdateRemainingEnemiesText(int remainingEnemies)
	{
        remainingEnemiesText.text = remainingEnemies.ToString();
	}

    public void UpdateCurrentWaveText(int currentWave)
	{
        currentWaveText.text = "WAVE " + currentWave;
	}

    public void Pause()
	{
		pauseManager.Pause(pauseScreen);
    }

	public void Resume()
	{
		pauseManager.Resume(pauseScreen);
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
