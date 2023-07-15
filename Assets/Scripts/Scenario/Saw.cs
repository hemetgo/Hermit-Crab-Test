using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
	[SerializeField] int damage;
	public int Damage { get => damage; }
	[SerializeField] float rotateSpeed;

	private void Update()
	{
		transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
	}
}
