using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ScoreManager
: GameControllerSystem<ScoreManager>
{
	//-- Constants
	private static readonly string SAVE_FILE_PATH = Application.persistentDataPath + "/scores.dat";

	//-- Settings
	[Tooltip( "Interval, in seconds, at which points are awarded automatically" )]
	public float m_TimerPointInterval = 1.0f;

	[Tooltip( "Number of points to award on every timer point interval" )]
	public float m_TimerPoints = 25.0f;

	//-- Attributes
	public int bestScore{ get{ return (int)m_BestScore; } }
	public int previousBestScore{ get{ return (int)m_PreviousBestScore; } }
	public int totalScore{ get{ return (int)(m_TimerScore + m_MiscScore); } }

	//-- Members
	private float m_BestScore = 0.0f;
	private float m_MiscScore = 0.0f;
	private float m_PreviousBestScore = 0.0f;
	private float m_TimerScore = 0.0f;

	void OnLevelWasLoaded()
	{
		m_MiscScore = 0.0f;
		m_TimerScore = 0.0f;
	}

	public void Load()
	{
		if( !File.Exists( SAVE_FILE_PATH ) )
		{
			return;
		}

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = File.Open( SAVE_FILE_PATH, FileMode.Open );

		ScoreData scoreData = (ScoreData)formatter.Deserialize( stream );
		m_BestScore = scoreData.bestScore;

		stream.Close();
	}

	public void Save()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = File.Open( SAVE_FILE_PATH, FileMode.OpenOrCreate );

		ScoreData scoreData;
		scoreData.bestScore = bestScore;

		formatter.Serialize( stream, scoreData );
		stream.Close();
	}

	void OnGameStateChange( GameController.GameStateEvent eventInfo )
	{
		if( GameController.instance.IsGamePlaying() )
		{
			StartCoroutine( AddTimerPoints() );
		}
		else if( GameController.GameState.GAME_OVER == eventInfo.currentState )
		{
			//-- This part uses getters to ignore fractions because
			//   the player can't see those
			int oldBestScore = bestScore;
			int sessionScore = totalScore;
			if( oldBestScore < sessionScore )
			{
				m_PreviousBestScore = m_BestScore;
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

	public void AddScore( uint scoreValue )
	{
		m_MiscScore += scoreValue;
	}
}

[System.Serializable]
struct ScoreData
{
	public int bestScore;
}
