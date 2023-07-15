using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIPlayerInput : MonoBehaviour
{
    [Inject] Player player;

    public void Fire()
	{
		player.Fire();
	}

	public void SwapDirection()
	{
		player.SwapDirection();
	}

	public void Jump()
	{
		player.Jump();
	}
}
