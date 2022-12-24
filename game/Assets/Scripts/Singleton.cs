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
