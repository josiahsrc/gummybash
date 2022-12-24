using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GummyBear : MonoBehaviour
{
	public CharacterMover mover;
	public Health health;
	public RadiusForceAdder force;
  public RadiusDamager damager;

	public static HashSet<GummyBear> bears = new();

	private void OnEnable()
	{
		health.onHealthChanged.AddListener(OnHealthChange);
		health.onDie.AddListener(OnDie);
		bears.Add(this);
	}

	private void OnDisable()
	{
		health.onHealthChanged.RemoveListener(OnHealthChange);
		health.onDie.RemoveListener(OnDie);
		bears.Remove(this);
	}

	private void OnHealthChange(int prev, int curr)
	{
    
	}

	private void OnDie()
	{
    bool win = true;
    foreach (GummyBear bear in bears)
    {
      if (bear.health.IsAlive)
      {
        win = false;
        break;
      }
    }

    if (win)
    {
      Debug.Log("GINGER BREAD HOUSE WINS");
    }
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && mover.enableComputerMovement)
		{
			force.Fire(transform.position);
      damager.Fire();
		}
	}
}
