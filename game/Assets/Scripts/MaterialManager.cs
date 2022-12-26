using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
	public Material sourceMat = null;
	public bool recursive = false;
	public List<GameObject> rendererObjs = null;
	public float flickerIntensity = .75f;

	private Coroutine _flickerRoutine = null;
	private Material _sharedMaterial = null;

	private HashSet<Renderer> GetRenderers()
	{
		var result = new HashSet<Renderer>();
		foreach (var obj in rendererObjs)
		{
			if (recursive)
			{
				var rends = obj.GetComponentsInChildren<Renderer>();
				foreach (var rend in rends)
				{
					result.Add(rend);
				}
			}
			else
			{
				var rend = obj.GetComponentInChildren<Renderer>();
				if (rend)
				{
					result.Add(rend);
				}
			}
		}

		return result;
	}

	public void AddRendObj(GameObject obj)
	{
		if (!rendererObjs.Contains(obj))
			rendererObjs.Add(obj);
	}

	public void RemoveRendObj(GameObject obj)
	{
		rendererObjs.Remove(obj);
	}

	public Material GetSharedMaterial()
	{
		if (_sharedMaterial)
			return _sharedMaterial;
		_sharedMaterial = new Material(sourceMat);
		return _sharedMaterial;
	}

	private void Start()
	{
		AssignMaterials();
	}

	public void AssignMaterials()
	{
		var sharedMat = GetSharedMaterial();
		var rends = GetRenderers();
		foreach (var rend in rends)
		{
			rend.sharedMaterial = sharedMat;
		}
	}

	public void StartFlicker(float duration)
	{
		StopFlicker();
		_flickerRoutine = StartCoroutine(Flicker(duration));
	}

	public void StopFlicker()
	{
		if (_flickerRoutine != null)
		{
			StopCoroutine(_flickerRoutine);
			ResetFlicker();
		}
		_flickerRoutine = null;
	}

	private void ResetFlicker()
	{
		GetSharedMaterial().SetColor("_EmissionColor", Color.black);
	}

	private IEnumerator Flicker(float duration)
	{
		const float FLICKER_RATE = 0.15f;

		GetSharedMaterial().SetColor("_EmissionColor", Color.black);

		bool prevWasBlack = true;
		while (duration > 0)
		{
			yield return new WaitForSeconds(FLICKER_RATE);

			duration -= FLICKER_RATE;
			if (prevWasBlack)
			{
				GetSharedMaterial().SetColor("_EmissionColor", Color.Lerp(Color.black, Color.white, flickerIntensity));
				prevWasBlack = false;
			}
			else
			{
				GetSharedMaterial().SetColor("_EmissionColor", Color.black);
				prevWasBlack = true;
			}
		}

		ResetFlicker();
		_flickerRoutine = null;
	}
}
