using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusDamager : MonoBehaviour
{
	public int damage = 1;
	public float radius = 1;
	public LayerMask layerMask;

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
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, GetScaledRadius());
	}
}
