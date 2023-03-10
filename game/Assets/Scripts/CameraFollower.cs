using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : Singleton<CameraFollower>
{
	public HashSet<CameraSubject> subjects = new HashSet<CameraSubject>();

	public float minDistance = 10;
	public float maxDistance = 30;
	public float deltaMult = 0.2f;
	public float smoothing = 0.2f;
  public float angle = 30f;
  public float angleSmoothing = 15f;
	public Vector3 offset = Vector3.zero;

	private Vector3 _vel = Vector3.zero;

	private void Start()
	{
		Sync();
	}

	private void Sync()
	{
		Vector3 target = Vector3.zero;
		var min = Vector3.positiveInfinity;
		var max = Vector3.negativeInfinity;
		foreach (var subject in subjects)
		{
			target += subject.transform.position;
      min = Vector3.Min(min, subject.transform.position);
      max = Vector3.Max(max, subject.transform.position);
		}
		target /= subjects.Count;
		target *= deltaMult;

		var distance = Vector3.Distance(min, max) + minDistance;
    distance = Mathf.Clamp(distance, minDistance, maxDistance);
    
    var top = new Vector3(target.x, distance, target.z);
    top.y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;
    top.z = -Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
		top += offset;

		var pos = Vector3.SmoothDamp(transform.position, top, ref _vel, smoothing);
		transform.position = pos;

    target.y = 0;
    var lookAt = target - pos;
    var rot = Quaternion.LookRotation(lookAt, Vector3.up);
    transform.rotation = Quaternion.Lerp(transform.rotation, rot, angleSmoothing * Time.deltaTime);
	}

	private void LateUpdate()
	{
		Sync();
	}
}
