using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RadiusForceAdder : MonoBehaviour
{
	public GameObject ignore;
	public LayerMask layerMask;
	public float radius = 1;
	public float force = 1;
	public float minMultipler = 0.1f;
	public FireEvent onFire = null;

	private float GetScaledRadius()
	{
		return radius * transform.lossyScale.x;
	}

	public void Fire(Vector3 origin)
	{
		var radius = GetScaledRadius();
		var hits = Physics.OverlapSphere(transform.position, radius, layerMask);
		foreach (var hit in hits)
		{
			var rb = hit.GetComponent<Rigidbody>();
			if (!rb || rb.gameObject == ignore)
				continue;

			var moverPos = rb.position;
			var diff = moverPos - origin;
			var multiplier = Mathf.Max(minMultipler, 1 - (diff.magnitude / radius));
			var forceDir = diff.normalized;
			rb.AddForce(forceDir * force * multiplier, ForceMode.Impulse);
		}

		onFire.Invoke(origin);
	}

	private void OnDrawGizmosSelected()
	{
		var radius = GetScaledRadius();
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	[System.Serializable]
	public class FireEvent : UnityEvent<Vector3>
	{

	}
}
