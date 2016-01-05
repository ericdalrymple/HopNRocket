using UnityEngine;
using System.Collections;

public class StaticAccessGameObject<T>
: MonoBehaviour
where T : StaticAccessGameObject<T>
{
	private static StaticAccessGameObject<T> s_Instance = null;

	public static T instance{ get{ return (T)s_Instance; } }

	void OnEnable()
	{
		s_Instance = this;
	}

	void OnDestroy()
	{
		s_Instance = null;
	}
}
