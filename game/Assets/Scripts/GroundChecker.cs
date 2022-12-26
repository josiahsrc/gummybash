using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
	public bool IsGrounded => count > 0;
	private int count = 0;

	private void OnTriggerEnter(Collider other)
	{
		count++;
	}

	private void OnTriggerExit(Collider other)
	{
		count--;
	}
}
