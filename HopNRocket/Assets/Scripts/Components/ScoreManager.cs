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

	private uint m_BestScore = 0;
	private uint m_CurrentScore = 0;

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
		print ( this.GetInstanceID() );
	}

	void Start()
	{

	}

	void Update()
	{
	
	}
}
