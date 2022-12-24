using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusForceAdder : MonoBehaviour
{
	public GameObject ignore;
	public LayerMask layerMask;
	public float radius = 1;
	public float force = 1;
	public float minMultipler = 0.1f;

	public void Fire(Vector3 origin)
	{
		var hits = Physics.OverlapSphere(origin, radius, layerMask);
		foreach (var hit in hits)
		{
			var mover = hit.GetComponent<CharacterMover>();
			if (!mover || mover.gameObject == ignore)
				continue;

			var moverPos = mover.transform.position;
			moverPos.y = origin.y;
			var diff = moverPos - origin;
			var multiplier = Mathf.Max(minMultipler, 1 - (diff.magnitude / radius));
			var forceDir = diff.normalized;
			mover.AddForce(forceDir * force * multiplier);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
