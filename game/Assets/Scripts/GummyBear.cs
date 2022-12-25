using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GummyBear : PlayerController
{
	public Health health;
	public RadiusForceAdder force;
	public RadiusDamager damager;
	public GameObject hammer;
	public Animator bearAnimator;

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

	protected override void Update()
	{
		base.Update();

		var speed = mover.rb.velocity.magnitude;
		bearAnimator.SetFloat("Movement", Mathf.Clamp01(speed / (mover.speed * .5f)));
	}

	protected override void OnButtonPressed()
	{
		bearAnimator.SetTrigger("Fire");
	}

	public void OnFire()
	{
		damager.Fire();
		force.Fire(transform.position);
	}
}
