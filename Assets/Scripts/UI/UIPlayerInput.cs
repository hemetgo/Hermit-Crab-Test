using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// Make the player actions buttons friendly
public class UIPlayerInput : MonoBehaviour
{
    [Inject] Player player;
    [Inject] PauseManager pauseManager;

    public void Fire()
	{
        if (pauseManager.IsPaused) return;

		player.Fire();
	}

	public void SwapDirection()
	{
        if (pauseManager.IsPaused) return;

		player.SwapDirection();
	}

	public void Jump()
	{
        if (pauseManager.IsPaused) return;

		player.Jump();
	}
}
