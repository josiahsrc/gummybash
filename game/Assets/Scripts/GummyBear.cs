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
	public Material sharedMaterial;
	public Color color;

	public GameObject bodyObj = null;
	public GameObject wormObj = null;

	public static HashSet<GummyBear> bears = new();

	private Color? lastColor;

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

		TryAssignMaterials();

		if (health.hitPoints == 0)
		{
			wormObj.SetActive(true);
			bodyObj.SetActive(false);
		}
		else if (health.hitPoints == 1)
		{
			wormObj.SetActive(false);
			bodyObj.SetActive(true);
		}
	}

	private void TryAssignMaterials()
	{
		var colorHex = player.user?.color ?? Utility.ColorToHex(color);
		if (colorHex == null)
			return;

		var newColor = Utility.HexToColor(colorHex);
		if (lastColor.HasValue && newColor == lastColor.Value)
			return;

		var material = new Material(sharedMaterial);
		material.color = newColor;

		var bodyRend = bodyObj.GetComponentInChildren<Renderer>();
		if (bodyRend != null)
			bodyRend.sharedMaterial = material;

		var wormRends = wormObj.GetComponentsInChildren<Renderer>();
		foreach (var rend in wormRends)
			rend.sharedMaterial = material;

		var worm = wormObj.GetComponentInChildren<WormEffect>();
		if (worm)
			worm.SetMaterial(material);
	}

	protected override void OnButtonPressed()
	{
		bearAnimator.SetTrigger("Fire");
	}

	public void OnFire()
	{
		damager.Fire();
		force.Fire(transform.position);
		bearAnimator.ResetTrigger("Fire");
	}
}
