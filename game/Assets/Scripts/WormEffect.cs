using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEffect : MonoBehaviour
{
	public GameObject bodyPartPrefab;
	public float bodySpeed = 10f;
	public int gap = 10;
	public int bodyParts = 5;
	public int maxPositions = 300;
	public MaterialManager materialManager = null;

	private List<Vector3> _positions = new();
	private List<GameObject> _bodyParts = new();

	private void Awake()
	{
		for (int i = 0; i < bodyParts; i++)
		{
			var part = Instantiate(bodyPartPrefab);
			part.transform.position = transform.position;
			part.transform.rotation = transform.rotation;
			_bodyParts.Add(part);
		}
		LinkToMaterialManager();
	}

	private void OnEnable()
	{
		_positions.Clear();
		foreach (var part in _bodyParts)
		{
			part.transform.position = transform.position;
			part.transform.rotation = transform.rotation;
			part.SetActive(true);
		}
	}

	private void OnDisable()
	{
		foreach (var part in _bodyParts)
		{
			if (part)
				part.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		UnlinkFromManager();
		foreach (var part in _bodyParts)
		{
			if (part)
				Destroy(part);
		}
	}

	public void SetMaterialManager(MaterialManager manager)
	{
		materialManager = manager;
		LinkToMaterialManager();
	}

	public void RemoveMaterialManager()
	{
		UnlinkFromManager();
		materialManager = null;
	}

	private void LinkToMaterialManager()
	{
		if (!materialManager)
			return;
		foreach (var obj in _bodyParts)
			materialManager.AddRendObj(obj);
	}

	private void UnlinkFromManager()
	{
		if (!materialManager)
			return;
		foreach (var obj in _bodyParts)
			materialManager.RemoveRendObj(obj);
	}

	void FixedUpdate()
	{
		var pos = transform.position;
		_positions.Insert(0, pos);

		var prevPositions = new List<Vector3>();
		for (int i = 0; i < _bodyParts.Count; i++)
		{
			prevPositions.Add(_bodyParts[i].transform.position);
		}

		for (int i = 0; i < _bodyParts.Count; i++)
		{
			var point = _positions[Mathf.Min(i * gap, _positions.Count - 1)];
			var dir = point - prevPositions[i];
			_bodyParts[i].transform.position += dir * Time.fixedDeltaTime * bodySpeed;
			_bodyParts[i].transform.rotation = dir == Vector3.zero ?
				Quaternion.identity : Quaternion.LookRotation(dir);
		}

    if (_positions.Count > maxPositions)
      _positions.RemoveAt(_positions.Count - 1);
	}
}
