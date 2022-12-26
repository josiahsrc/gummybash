using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public int hitPoints = 1;
	public int maxHealth = 1;
	public MaterialManager materialManager = null;
	public float invincibilityDuration = 2f;

	public UnityEvent onDie = null;
	public HealthChangedEvent onHealthChanged = null;

	private Coroutine invincibilityCoroutine = null;

	public bool IsAlive => hitPoints > 0;
	public bool IsInvincible => invincibilityCoroutine != null;

	public void ModifyHealth(int delta)
	{
		if (delta < 0 && IsInvincible)
			return;

		var prevHitPoints = hitPoints;
		hitPoints += delta;
		hitPoints = Mathf.Clamp(hitPoints, 0, maxHealth);

		if (prevHitPoints != hitPoints)
			onHealthChanged?.Invoke(prevHitPoints, hitPoints);
		if (hitPoints <= 0)
			onDie?.Invoke();
	}

	public void StartInvincibility()
	{
		StopInvincibility();
		invincibilityCoroutine = StartCoroutine(DoInvincibilityRoutine());
		if (materialManager)
			materialManager.StartFlicker(invincibilityDuration);
	}

	public void StopInvincibility()
	{
		if (invincibilityCoroutine != null)
		{
			StopCoroutine(invincibilityCoroutine);
			if (materialManager)
				materialManager.StopFlicker();
		}
		invincibilityCoroutine = null;
	}

	private IEnumerator DoInvincibilityRoutine()
	{
		yield return new WaitForSeconds(invincibilityDuration);
		invincibilityCoroutine = null;
		if (materialManager)
			materialManager.StopFlicker();
	}

	[System.Serializable]
	public class HealthChangedEvent : UnityEvent<int, int> { }
}
