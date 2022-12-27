using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GummyBear : PlayerController
{
	public MaterialManager hammerMaterialManager = null;
	public MaterialManager gummyMaterialManager = null;

	public Health health;
	public RadiusForceAdder force;
	public RadiusDamager damager;
	public GameObject hammer;
	public Animator bearAnimator;
	public Color color;

	public float bigScaleMultiplier = 1.5f;
	public float scaleRate = 30f;
	public float bigDamageMultipler = 1.5f;
	public float bigForceMultiplier = 1.5f;
	public float bigMassMultiplier = 3f;

	public PhysicMaterial wormMaterial;
	public PhysicMaterial normalMaterial;
	public PhysicMaterial bigMaterial;

	public float hammerTime = 10f;
	public float bigTime = 10f;
	public float flickerStartTime = 1.5f;

	public GameObject bodyObj = null;
	public GameObject wormObj = null;

	public static HashSet<GummyBear> bears = new();

	private Color? lastColor;
	private Vector3 initialScale;
	private int initialDamage = 0;
	private float initialForce = 0;
	private Coroutine hammerCoroutine = null;
	private Coroutine bigCoroutine = null;
	private float initialMass = 0f;

	private bool HasHammer => hammerCoroutine != null;
	private bool IsBig => bigCoroutine != null;

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

	private void Awake()
	{
		SyncHammer();
	}

	private void StopHammer()
	{
		if (HasHammer)
		{
			StopCoroutine(hammerCoroutine);
			hammerMaterialManager.StopFlicker();
		}
		hammerCoroutine = null;
	}

	private void StartHammer()
	{
		StopHammer();
		hammerCoroutine = StartCoroutine(HammerRemoveRoutine());
	}

	private void StopBig()
	{
		if (IsBig)
		{
			StopCoroutine(bigCoroutine);
			gummyMaterialManager.StopFlicker();
		}
		bigCoroutine = null;
	}

	private void StartBig()
	{
		StopBig();
		bigCoroutine = StartCoroutine(BigPowerDownRoutine());
	}

	private void SyncHammer()
	{
		var hasHammer = HasHammer && health.hitPoints > 0;
		if (!hasHammer)
			StopHammer();
		hammer.SetActive(hasHammer);
	}

	private void Start()
	{
		initialScale = transform.localScale;
		initialDamage = damager.damage;
		initialForce = force.force;
		initialMass = mover.rb.mass;
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
			Server.Instance.SendWinnerRequest(UserType.gingerBreadHouse);
		}
	}

	protected override void Update()
	{
		base.Update();

		bearAnimator.SetFloat("Movement", mover.smoothJoystick.magnitude);

		TryAssignMaterials();

    gameObject.layer = LayerMask.NameToLayer(health.hitPoints == 0 ? "Gummy Worm" : "Gummy Bear");

		Vector3 targetScale;
		if (health.hitPoints <= 1)
		{
			wormObj.SetActive(health.hitPoints == 0);
			bodyObj.SetActive(health.hitPoints == 1);
			mover.col.sharedMaterial = health.hitPoints == 0 ? wormMaterial : normalMaterial;
			targetScale = initialScale;
			damager.damage = initialDamage;
			force.force = initialForce;
			mover.rb.mass = initialMass;
			mover.useControlledVelocity = health.hitPoints == 0;
			mover.randomJoystick = health.hitPoints == 0;
		}
		else if (health.hitPoints == 2)
		{
			wormObj.SetActive(false);
			bodyObj.SetActive(true);
			mover.col.sharedMaterial = bigMaterial;
			targetScale = initialScale * bigScaleMultiplier;
			damager.damage = Mathf.RoundToInt(initialDamage * bigDamageMultipler);
			force.force = initialForce * bigForceMultiplier;
			mover.rb.mass = initialMass * bigMassMultiplier;
			mover.useControlledVelocity = false;
			mover.randomJoystick = false;
		}
		else
		{
			throw new System.NotImplementedException();
		}

		SyncHammer();

		transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleRate);
	}

	private void TryAssignMaterials()
	{
		var colorHex = player.user?.color ?? Utility.ColorToHex(color);
		if (colorHex == null)
			return;

		var newColor = Utility.HexToColor(colorHex);
		if (lastColor.HasValue && newColor == lastColor.Value)
			return;

		var sharedMaterial = gummyMaterialManager.GetSharedMaterial();
		sharedMaterial.color = newColor;
		gummyMaterialManager.AddRendObj(bodyObj);

		var worm = wormObj.GetComponentInChildren<WormEffect>();
		if (worm)
			worm.SetMaterialManager(gummyMaterialManager);

		gummyMaterialManager.AssignMaterials();
	}

	protected override void OnButtonPressed()
	{
		if (HasHammer)
			bearAnimator.SetTrigger("Fire");
	}

	public void OnFire()
	{
		if (HasHammer)
		{
			damager.Fire();
			force.Fire(transform.position);
		}

		bearAnimator.ResetTrigger("Fire");
	}

	private void OnTriggerEnter(Collider other)
	{
		var healthPickUp = other.GetComponent<HealthPickUp>();
		if (healthPickUp)
		{
			health.ModifyHealth(healthPickUp.modifier);
			Destroy(healthPickUp.gameObject);
			if (health.hitPoints == 2)
				StartBig();
		}

		var hammerPickUp = other.GetComponent<HammerPickUp>();
		if (hammerPickUp && health.hitPoints > 0)
		{
			StartHammer();
			Destroy(hammerPickUp.gameObject);
		}
	}

	private IEnumerator HammerRemoveRoutine()
	{
		var firstHalf = hammerTime - flickerStartTime;
		var secondHalf = flickerStartTime;
		yield return new WaitForSeconds(firstHalf);
		hammerMaterialManager.StartFlicker(secondHalf);
		yield return new WaitForSeconds(secondHalf);
		hammerMaterialManager.StopFlicker();
		hammerCoroutine = null;
	}

	private IEnumerator BigPowerDownRoutine()
	{
		var firstHalf = bigTime - flickerStartTime;
		var secondHalf = flickerStartTime;
		yield return new WaitForSeconds(firstHalf);
		gummyMaterialManager.StartFlicker(secondHalf);
		yield return new WaitForSeconds(secondHalf);
		health.ModifyHealth(-1);
		gummyMaterialManager.StopFlicker();
		bigCoroutine = null;
	}
}
