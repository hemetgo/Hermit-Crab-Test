using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealthBar : MonoBehaviour
{
	[Inject] Player player;

	[SerializeField] Image healthTilePrefab;
	[SerializeField] Transform tilesParent;
	[SerializeField] Color filledColor;
	[SerializeField] Color emptyColor;

	List<Image> healthTiles = new List<Image>();

	private void Start()
	{
		CreateTiles();
	}

	// Create hearts on UI
	void CreateTiles()
	{
		for (int i = 0; i < player.Health.MaxHealth; i++)
		{
			Image tile = Instantiate(healthTilePrefab, tilesParent);
			healthTiles.Add(tile);
		}
		UpdateBar();
	}

	// Update hearts based on current player health
	public void UpdateBar()
	{
		for (int i = 0; i < player.Health.MaxHealth; i++)
		{
			healthTiles[i].color = i < player.Health.CurrentHealth ? filledColor : emptyColor;
		}
	}
}
