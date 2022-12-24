using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public int hitPoints = 1;
  public int maxHealth = 1;

	public UnityEvent onDie = null;
	public HealthChangedEvent onHealthChanged = null;

  public bool IsAlive => hitPoints > 0;

  public void ModifyHealth(int delta)
  {
    var prevHitPoints = hitPoints;

    hitPoints += delta;
    hitPoints = Mathf.Clamp(hitPoints, 0, maxHealth);

    if (prevHitPoints != hitPoints)
      onHealthChanged?.Invoke(prevHitPoints, hitPoints);
    if (hitPoints <= 0)
      onDie?.Invoke();
  }

  [System.Serializable]
  public class HealthChangedEvent : UnityEvent<int, int> { }
}
