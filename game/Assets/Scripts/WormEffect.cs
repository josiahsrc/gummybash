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
	}

	private void OnEnable()
	{
		foreach (var part in _bodyParts)
			part.SetActive(true);
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
		foreach (var part in _bodyParts)
		{
			if (part)
				Destroy(part);
		}
	}

	public void SetMaterial(Material mat)
	{
		foreach (var part in _bodyParts)
		{
			var rend = part.GetComponent<Renderer>();
			if (rend)
				rend.sharedMaterial = mat;
		}
	}

	void Update()
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
			_bodyParts[i].transform.position += dir * Time.deltaTime * bodySpeed;
			_bodyParts[i].transform.rotation = dir == Vector3.zero ?
				Quaternion.identity : Quaternion.LookRotation(dir);
		}

    if (_positions.Count > maxPositions)
      _positions.RemoveAt(_positions.Count - 1);
	}
}
