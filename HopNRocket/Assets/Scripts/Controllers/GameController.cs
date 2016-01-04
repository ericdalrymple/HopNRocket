using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class GameController
: MonoBehaviour
{
	//-- States for the game's overall state machine
	public enum GameState
	{
		  NONE = 0
		, TITLE_SCREEN
		, PLAYING
		, GAME_OVER
	}

	//-- Struct for information relevant to game state events
	public struct GameStateEvent
	{
		public GameState previousState;
		public GameState currentState;
	}

	//-- Messages sent by this MonoBehaviour
	private static readonly string MESSAGE_GAME_STATE_CHANGE = "OnGameStateChange";

	//-- Singleton instance
	private static GameController s_Instance;

	public static GameController instance
	{
		get{ return s_Instance; }
	}

	//-- Member variables
	private GameObject[] m_GameStateListeners;
	private GameState m_CurrentGameState;

	void Awake()
	{
		//-- Singleton logic for component and parent game object
		if( null == s_Instance )
		{
			s_Instance = this;
			DontDestroyOnLoad( gameObject );
		}
		else
		{
			Destroy( gameObject );
		}

		LoadGame();
	}

	void OnLevelWasLoaded()
	{
		if( this == s_Instance )
		{
			Initialize();
		}
	}

	void Start()
	{
		//-- Initialize game state to "NONE"
		m_CurrentGameState = GameState.NONE;

		Initialize();
	}

	void Update()
	{
		switch( m_CurrentGameState )
		{
			case GameState.TITLE_SCREEN:
			{
				if( Input.anyKeyDown )
				{
					SetGameState( GameState.PLAYING );
				}

				break;
			}

			case GameState.PLAYING:
			{
				break;
			}

			case GameState.GAME_OVER:
			{
				if( Input.anyKeyDown )
				{
					Application.LoadLevel( Application.loadedLevel );
				}

				break;
			}
		}
	}

	public void LoadGame()
	{
		ScoreManager.instance.Load();
	}

	public void SaveGame()
	{
		ScoreManager.instance.Save();
	}

	public bool IsGamePlaying()
	{
		return (GameState.PLAYING == m_CurrentGameState);
	}

	void Initialize()
	{
		//-- Initialize an array will all the objects we
		//   want to notify about game state
		m_GameStateListeners = new GameObject[4];
		{
			m_GameStateListeners[0] = gameObject;
			m_GameStateListeners[1] = GameObject.FindGameObjectWithTag( "Player" );
			m_GameStateListeners[2] = GameObject.FindGameObjectWithTag( "UI" );
			m_GameStateListeners[3] = GameObject.FindGameObjectWithTag( "World" );
		}

		//-- Initialize game state
		SetGameState( GameState.TITLE_SCREEN );
	}

	void OnPlayerDead()
	{
		//-- Game over if the player dies
		SetGameState( GameState.GAME_OVER );
	}

	void NotifyStateChange( GameState oldState, GameState newState )
	{
		//-- Prepare event information
		GameStateEvent eventInfo = new GameStateEvent();
		{
			eventInfo.previousState = oldState;
			eventInfo.currentState = newState;
		}

		//-- Send the event
		foreach( GameObject listener in m_GameStateListeners )
		{
			listener.BroadcastMessage( MESSAGE_GAME_STATE_CHANGE
			                         , eventInfo
			                         , SendMessageOptions.DontRequireReceiver );
		}
	}

	void SetGameState( GameState state )
	{
		if( state == m_CurrentGameState )
		{
			//-- Ignore changes to the current state
			return;
		}

		GameState backup = m_CurrentGameState;
		m_CurrentGameState = state;

		NotifyStateChange( backup, m_CurrentGameState );

		//-- Setup
		switch( m_CurrentGameState )
		{
			case GameState.TITLE_SCREEN:
			{
				break;
			}
				
			case GameState.PLAYING:
			{
				break;
			}
				
			case GameState.GAME_OVER:
			{
				SaveGame();
				break;
			}
				
			default:
			{
				Assert.IsTrue( false, "Invalid game state: " + m_CurrentGameState );
				break;
			}
		}
	}
}
