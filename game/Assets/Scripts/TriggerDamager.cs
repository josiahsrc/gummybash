using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamager : MonoBehaviour
{
	public int damage = 1;
	public LayerMask layerMask;

	private void OnTriggerEnter(Collider other)
	{
		var targetable = other.GetComponent<Health>();
		if (!targetable)
			return;

    var canHit = layerMask == (layerMask | (1 << other.gameObject.layer));
    if (!canHit)
      return;

		targetable.ModifyHealth(-damage);
	}
}
