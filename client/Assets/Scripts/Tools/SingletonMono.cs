using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	public virtual bool IsDontDestroyOnLoad { get; set; } = false;
	public static bool IsInitialized
	{
		get { return _instance != null; }
	}

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				MonoBehaviour[] instances = FindObjectsOfType<T>();

				if (instances.Length > 0) _instance = (T)instances[0];
				if (instances.Length > 1)
				{
					Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton!");
					return _instance;
				}
				if (_instance == null)
				{
					Debug.LogError("[Singleton] Something went really wrong - specified Singleton does not found!");
					return default;
				}
			}
			return _instance;
		}
		set
		{
			if (_instance == null) _instance = value;
		}
	}

	public virtual void Awake()
	{
		if (_instance == null)
		{
			Instance = this as T;
			if (IsDontDestroyOnLoad) DontDestroyOnLoad(gameObject);
		}
	}
}
