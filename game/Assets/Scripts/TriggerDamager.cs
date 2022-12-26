using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDamager : MonoBehaviour
{
	public int damage = 1;
	public LayerMask layerMask;
	public OnDamageEvent onDamage;

	private void OnTriggerEnter(Collider other)
	{
		var targetable = other.GetComponent<Health>();
		if (!targetable)
			return;

		var canHit = layerMask == (layerMask | (1 << other.gameObject.layer));
		if (!canHit)
			return;

		targetable.ModifyHealth(-damage);
		onDamage?.Invoke(other.gameObject);
	}

	[System.Serializable]
	public class OnDamageEvent : UnityEvent<GameObject> { }
}
