using UnityEngine;
using System.Collections;

/**
 * Useful class for getting partial Singleton functionality in
 * scripts that are in the same game object as GameController,
 * which exclusively handles the singleton logic for its game
 * object.
 */
public abstract class GameControllerSystem<T>
: MonoBehaviour
where T : GameControllerSystem<T>
{
	//-- Singleton code
	private static GameControllerSystem<T> s_Instance = null;
	
	public static bool initialized{ get{ return (null != s_Instance); } }
	public static T instance{ get{ return (T)s_Instance; } }

	void OnEnable()
	{
		if( !initialized )
		{
			s_Instance = this;
		}
	}
}
