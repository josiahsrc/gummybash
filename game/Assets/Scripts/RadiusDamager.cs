using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RadiusDamager : MonoBehaviour
{
	public int damage = 1;
	public float radius = 1;
	public LayerMask layerMask;
	public UnityEvent onFire = null;

  private float GetScaledRadius()
  {
    return radius * transform.lossyScale.x;
  }

	public void Fire()
	{
		var hits = Physics.OverlapSphere(transform.position, GetScaledRadius(), layerMask);
		foreach (var hit in hits)
		{
			var targetable = hit.GetComponent<Health>();
			if (!targetable)
				continue;
			targetable.ModifyHealth(-damage);
		}
		onFire?.Invoke();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, GetScaledRadius());
	}
}
