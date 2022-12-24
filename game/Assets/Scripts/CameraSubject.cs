using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSubject : MonoBehaviour
{
	private void OnEnable()
	{
		if (CameraFollower.Instance)
		{
			CameraFollower.Instance.subjects.Add(this);
		}
	}

	private void OnDisable()
	{
		if (CameraFollower.Instance)
		{
			CameraFollower.Instance.subjects.Remove(this);
		}
	}
}
