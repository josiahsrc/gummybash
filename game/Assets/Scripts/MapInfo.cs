using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapInfo : Singleton<MapInfo>
{
	public float sampleDistance = 20f;
	public BoxCollider col;

	public Vector3 RandomPosition()
	{
		var minPos = col.bounds.min;
		var maxPos = col.bounds.max;
		return new Vector3(
			Random.Range(minPos.x, maxPos.x),
			Random.Range(minPos.y, maxPos.y),
			Random.Range(minPos.z, maxPos.z)
		);
	}

	public Vector3 RandomPositionOnNavMesh()
	{
		var pos = RandomPosition();
		if (NavMesh.SamplePosition(pos, out var hit, sampleDistance, NavMesh.AllAreas))
		{
			return hit.position;
		}

		return pos;
	}
}
