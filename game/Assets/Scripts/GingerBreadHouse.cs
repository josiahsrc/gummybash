using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GingerBreadHouse : PlayerController
{
	public Health health;
	public Animator animator;
	public float dashForce = 10f;
	public float dashCooldown = .75f;
	public float dashDuration = 0.7f;
	public BoxCollider dashDamagerCol = null;
	public TriggerDamager dashDamager = null;
	public Transform buttHole = null;
	public float poopForce = 5f;
	public int healthPerEat = 1;

	private float dashTime = 0f;
	private Coroutine dashCoroutine = null;
	private Stack<GameObject> poopStack = new Stack<GameObject>();
	public GingerBreadHouseInfo info = null;

	private void OnEnable()
	{
		health.onHealthChanged.AddListener(OnHealthChange);
		health.onDie.AddListener(OnDie);
		dashDamagerCol.enabled = false;
		dashDamager.onDamage.AddListener(OnEatSomething);

		info.health = health.hitPoints;
		info.maxHealth = health.maxHealth;
	}

	private void OnDisable()
	{
		health.onHealthChanged.RemoveListener(OnHealthChange);
		health.onDie.RemoveListener(OnDie);
		dashDamager.onDamage.RemoveListener(OnEatSomething);
	}

	private void OnHealthChange(int prev, int curr)
	{
		info.health = curr;
	}

	private void StartDash()
	{
		if (dashCoroutine != null)
			StopCoroutine(dashCoroutine);
		dashCoroutine = StartCoroutine(Dash());
	}

	private IEnumerator Dash()
	{
		var fwd = transform.forward;
		mover.rb.AddForce(fwd * dashForce, ForceMode.Impulse);
		dashTime = dashCooldown;

		animator.SetBool("Dash", true);
		dashDamagerCol.enabled = true;
		yield return new WaitForSeconds(dashDuration);
		animator.SetBool("Dash", false);

		dashCoroutine = null;
	}

	private void OnDie()
	{
		Server.Instance.SendWinnerRequest(UserType.gummyBear);
	}

	protected override void Update()
	{
		base.Update();
		animator.SetFloat("Movement", mover.smoothJoystick.magnitude);
		animator.SetBool("Poop", poopStack.Count > 0);
		dashTime -= Time.deltaTime;
		dashDamagerCol.enabled = dashCoroutine != null;
	}

	protected override void OnButtonPressed()
	{
		if (dashTime < 0)
			StartDash();
	}

	private void OnEatSomething(GameObject target)
	{
		target.SetActive(false);
		poopStack.Push(target);
		health.ModifyHealth(healthPerEat);
	}

	public void PoopStack()
	{
		if (!poopStack.TryPop(out var res))
			return;

		res.SetActive(true);
		res.transform.position = buttHole.transform.position;
		res.transform.rotation = buttHole.transform.rotation;

		var rb = res.GetComponent<Rigidbody>();
		if (rb)
			rb.AddForce(buttHole.forward * poopForce, ForceMode.Impulse);

		var mover = res.GetComponent<CharacterMover>();
		if (mover)
			mover.BreakControlsTemporarily();
	}

	private void OnTriggerEnter(Collider other)
	{
		var isAlive = health.hitPoints > 0;
		var healthPickUp = other.GetComponent<HealthPickUp>();
		if (healthPickUp && isAlive)
		{
			health.ModifyHealth(healthPickUp.modifier);
			Destroy(healthPickUp.gameObject);
		}
	}
}
