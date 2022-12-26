using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GingerBreadHouse : PlayerController
{
	public Health health;
	public Animator animator;
	public float dashForce = 10f;
	public float dashCooldown = .75f;

	private float dashTime = 0f;

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

	protected override void Update()
	{
		base.Update();
		animator.SetFloat("Movement", mover.smoothJoystick.magnitude);
		dashTime -= Time.deltaTime;
	}

	protected override void OnButtonPressed()
	{
		if (dashTime < 0)
		{
			animator.SetTrigger("Fire");
			var fwd = transform.forward;
			mover.rb.AddForce(fwd * dashForce, ForceMode.Impulse);
			dashTime = dashCooldown;
		}
	}
}
