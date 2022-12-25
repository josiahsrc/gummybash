using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyRotator : MonoBehaviour
{
	public float bounceSpeed = 1f;
	public float bounceHeight = 1f;
	public float rotationSpeed = 1f;

	void LateUpdate()
	{
		transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * bounceSpeed) * bounceHeight, 0);
		transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
	}
}
