using UnityEngine;
using System.Collections;

public class ScoreManager
: MonoBehaviour
{
	private static ScoreManager s_Instance = null;

	public static ScoreManager instance
	{
		get{ return s_Instance; }
	}

	void Awake()
	{
		//-- Apply singleton logic upon object creation; only one
		//   score manager should persist per game session.
		if( null == s_Instance )
		{
			s_Instance = this;
			DontDestroyOnLoad( gameObject );
		}
		else
		{
			Destroy( gameObject );
		}
	}

	void OnLevelWasLoaded()
	{

	}

	void Start()
	{

	}

	void Update()
	{
	
	}
}
