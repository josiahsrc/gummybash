using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	public GameObject prefab = null;
	public int maxAlive = 2;
	public float minSpawnInterval = 5f;
	public float maxSpawnInterval = 10f;
	public float yOffset = 1f;

	private Coroutine spawnRoutine = null;
	private HashSet<GameObject> aliveObjs = new();

	private void OnEnable()
	{
		spawnRoutine = StartCoroutine(Spin());
	}

	private void OnDisable()
	{
		StopCoroutine(spawnRoutine);
		spawnRoutine = null;
	}

	private void SyncAliveObjs()
	{
		var objs = aliveObjs.ToHashSet();
		foreach (var obj in objs)
		{
			if (!obj)
				aliveObjs.Remove(obj);
		}
	}

	private IEnumerator Spin()
	{
		while(true)
		{
			SyncAliveObjs();
			if (aliveObjs.Count >= maxAlive)
				continue;

			var point = MapInfo.Instance.RandomPositionOnNavMesh();
			var obj = Instantiate(prefab);
			point.y += yOffset;
			obj.transform.position = point;
			aliveObjs.Add(obj);

			var delay = Random.Range(minSpawnInterval, maxSpawnInterval);
			yield return new WaitForSeconds(delay);
		}
	}
}
