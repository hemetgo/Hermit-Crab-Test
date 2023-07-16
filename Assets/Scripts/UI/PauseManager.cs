using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control the pause system 
public class PauseManager
{
	bool isPaused;
	public bool IsPaused { get => isPaused; }
	
	public void Pause(GameObject pauseScreen)
	{
		isPaused = true;
		pauseScreen.SetActive(true);
		Time.timeScale = 0;
	}

	public void Resume(GameObject pauseScreen)
	{
		isPaused = false;
		pauseScreen.SetActive(false);
		Time.timeScale = 1;
	}
}
