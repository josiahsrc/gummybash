using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
	public UnityEvent onFire;
	public UnityEvent onPoop;

	public void Fire()
	{
		onFire?.Invoke();
	}

	public void Poop()
	{
		onPoop?.Invoke();
	}
}
