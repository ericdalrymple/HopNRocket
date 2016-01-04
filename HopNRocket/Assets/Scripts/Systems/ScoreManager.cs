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

	[Tooltip( "Interval, in seconds, at which points are awarded automatically" )]
	public float m_TimerPointInterval = 1.0f;

	[Tooltip( "Number of points to award on every timer point interval" )]
	public float m_TimerPoints = 25.0f;

	private float m_BestScore = 0.0f;
	private float m_TimerScore = 0.0f;

	void Awake()
	{
		//-- Apply singleton logic upon object creation; only one
		//   score manager should persist per game session.
		if( null == s_Instance )
		{
			s_Instance = this;
		}
	}

	void OnLevelWasLoaded()
	{
		m_TimerScore = 0.0f;
	}

	public int GetBestScore()
	{
		return (int)m_BestScore;
	}

	public int GetTotalScore()
	{
		return (int)m_TimerScore;
	}

	void OnGameStateChange( GameController.GameStateEvent eventInfo )
	{
		if( GameController.instance.IsGamePlaying() )
		{
			StartCoroutine( AddTimerPoints() );
		}
		else if( GameController.GameState.GAME_OVER == eventInfo.currentState )
		{
			int oldBestScore = GetBestScore();
			int sessionScore = GetTotalScore();
			if( oldBestScore < sessionScore )
			{
				m_BestScore = sessionScore;
			}
		}
	}

	IEnumerator AddTimerPoints()
	{
		do
		{
			yield return new WaitForSeconds( m_TimerPointInterval );
			m_TimerScore += m_TimerPoints;
		}
		while( GameController.instance.IsGamePlaying() );
	}
}
