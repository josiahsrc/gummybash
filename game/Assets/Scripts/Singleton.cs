using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;

	public static T Instance
	{
		get
		{
			if (!_instance)
			{
				_instance = FindObjectOfType<T>();
			}

			return _instance;
		}
	}
}

public abstract class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static GameObject _obj = null;
	private static T _instance = null;

	public static T Instance
	{
		get
		{
			if (!_obj)
			{
				_obj = new GameObject("Singleton");
				DontDestroyOnLoad(_obj);
			}

			if (!_instance)
				_instance = FindObjectOfType<T>();

			if (!_instance)
				_instance = _obj.AddComponent<T>();

			return _instance;
		}
	}
}
