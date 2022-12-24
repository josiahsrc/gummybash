using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GingerBreadHouse : Singleton<GingerBreadHouse>
{
	public CharacterMover mover;
	public Health health;

	private void OnEnable()
	{
		health.onHealthChanged.AddListener(OnHealthChange);
		health.onDie.AddListener(OnDie);
	}

	private void OnDisable()
	{
		health.onHealthChanged.RemoveListener(OnHealthChange);
		health.onDie.RemoveListener(OnDie);
	}

	private void OnHealthChange(int prev, int curr)
	{

	}

	private void OnDie()
	{
		print("WINNER IS BEARS!!!!");
	}
}
