using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		var health = other.GetComponent<Health>();
		if (!health)
			return;

		var pos = MapInfo.Instance.RandomPositionOnNavMesh();
		health.StartInvincibility();
		other.gameObject.transform.position = pos;
		other.gameObject.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);

		var rb = other.GetComponent<Rigidbody>();
		if (rb)
			rb.velocity = Vector3.zero;
	}
}
