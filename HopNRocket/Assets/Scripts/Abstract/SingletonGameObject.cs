﻿using UnityEngine;
using System.Collections;

/**
 * Superclass for game object controller scripts that need to
 * persist across levels and only have one active instance.
 */
public class SingletonGameObject<T>
: MonoBehaviour
where T : SingletonGameObject<T>
{
	private static SingletonGameObject<T> s_Instance = null;
	
	public static T instance
	{
		get
		{
			return (T)s_Instance;
		}
	}
	
	void OnEnable()
	{
		if( null == instance )
		{
			s_Instance = this;
			DontDestroyOnLoad( gameObject );
		}
		else
		{
			Destroy( gameObject );
		}
	}
}
