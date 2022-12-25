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

	public float bigScaleMultiplier = 1.5f;
	public float scaleRate = 30f;
	public float bigDamageMultipler = 1.5f;
	public float bigForceMultiplier = 1.5f;
	public float bigSpeedMultiplier = 1.3f;
	public float bigMassMultiplier = 3f;

	public GameObject hammerObj = null;
	public float hammerTime = 10f;

	public float bigTime = 10f;

	public float flickerRate = 0.3f;
	public float flickerStartTime = 1.5f;

	public GameObject bodyObj = null;
	public GameObject wormObj = null;

	public static HashSet<GummyBear> bears = new();

	private Color? lastColor;
	private Vector3 initialScale;
	private int initialDamage = 0;
	private float initialForce = 0;
	private float initialSpeed = 0;
	private Coroutine hammerCoroutine = null;
	private Coroutine bigCoroutine = null;
	private Material hammerMat = null;
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

		var hammerRend = hammerObj.GetComponentInChildren<Renderer>();
		hammerMat = new Material(hammerRend.sharedMaterial);
		hammerRend.sharedMaterial = hammerMat;
	}

	private void StopHammer()
	{
		if (HasHammer)
			StopCoroutine(hammerCoroutine);
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
			StopCoroutine(bigCoroutine);
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
		initialSpeed = mover.speed;
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
			Debug.Log("GINGER BREAD HOUSE WINS");
		}
	}

	protected override void Update()
	{
		base.Update();

		var speed = mover.rb.velocity.magnitude;
		bearAnimator.SetFloat("Movement", Mathf.Clamp01(speed / (mover.speed * .5f)));

		TryAssignMaterials();

		Vector3 targetScale;
		if (health.hitPoints <= 1)
		{
			wormObj.SetActive(health.hitPoints == 0);
			bodyObj.SetActive(health.hitPoints == 1);
			targetScale = initialScale;
			damager.damage = initialDamage;
			force.force = initialForce;
			mover.speed = initialSpeed;
			mover.rb.mass = initialMass;
		}
		else if (health.hitPoints == 2)
		{
			wormObj.SetActive(false);
			bodyObj.SetActive(true);
			targetScale = initialScale * bigScaleMultiplier;
			damager.damage = Mathf.RoundToInt(initialDamage * bigDamageMultipler);
			force.force = initialForce * bigForceMultiplier;
			mover.speed = initialSpeed * bigSpeedMultiplier;
			mover.rb.mass = initialMass * bigMassMultiplier;
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

		sharedMaterial = new Material(sharedMaterial);
		sharedMaterial.color = newColor;

		var bodyRend = bodyObj.GetComponentInChildren<Renderer>();
		if (bodyRend != null)
			bodyRend.sharedMaterial = sharedMaterial;

		var wormRends = wormObj.GetComponentsInChildren<Renderer>();
		foreach (var rend in wormRends)
			rend.sharedMaterial = sharedMaterial;

		var worm = wormObj.GetComponentInChildren<WormEffect>();
		if (worm)
			worm.SetMaterial(sharedMaterial);
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
		hammerMat.SetColor("_EmissionColor", Color.black);

		bool prevWasBlack = true;
		var duration = hammerTime;
		while (duration > 0)
		{
			yield return new WaitForSeconds(flickerRate);

			duration -= flickerRate;
			if (!HasHammer)
				break;

			if (duration < flickerStartTime)
			{
				if (prevWasBlack)
				{
					hammerMat.SetColor("_EmissionColor", Color.white);
					prevWasBlack = false;
				}
				else
				{
					hammerMat.SetColor("_EmissionColor", Color.black);
					prevWasBlack = true;
				}
			}
		}

		hammerMat.SetColor("_EmissionColor", Color.black);
		hammerCoroutine = null;
	}

	private IEnumerator BigPowerDownRoutine()
	{
		sharedMaterial.SetColor("_EmissionColor", Color.black);

		bool prevWasBlack = true;
		var duration = bigTime;
		while (duration > 0)
		{
			yield return new WaitForSeconds(flickerRate);

			duration -= flickerRate;
			if (!IsBig)
				break;

			if (duration < flickerStartTime)
			{
				if (prevWasBlack)
				{
					sharedMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.white, .75f));
					prevWasBlack = false;
				}
				else
				{
					sharedMaterial.SetColor("_EmissionColor", Color.black);
					prevWasBlack = true;
				}
			}
		}

		sharedMaterial.SetColor("_EmissionColor", Color.black);
		health.ModifyHealth(-1);
		bigCoroutine = null;
	}
}
