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

	public static GameController Instance
	{
		get{ return s_Instance; }
	}

	//-- Member variables
	private GameObject m_Player;
	private GameObject m_UIManager;
	private GameObject m_World;
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

	void Initialize()
	{
		//-- Cache player object
		m_Player = GameObject.FindGameObjectWithTag( "Player" );
		
		//-- Cache UI manager object
		m_UIManager = GameObject.FindGameObjectWithTag( "UI" );
		
		//-- Cache world object
		m_World = GameObject.FindGameObjectWithTag( "World" );
		
		//-- Initialize game state
		SetGameState( GameState.TITLE_SCREEN );
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
		if( null != m_Player )
		{
 			m_Player.BroadcastMessage( MESSAGE_GAME_STATE_CHANGE
			                         , eventInfo
			                         , SendMessageOptions.DontRequireReceiver );
		}

		if( null != m_UIManager )
		{
			m_UIManager.BroadcastMessage( MESSAGE_GAME_STATE_CHANGE
			                            , eventInfo
			                            , SendMessageOptions.DontRequireReceiver );
		}
		
		if( null != m_World )
		{
			m_World.BroadcastMessage( MESSAGE_GAME_STATE_CHANGE
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

		//-- Initialize stuff for the 
		switch( state )
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
			break;
		}
			
		default:
		{
			Assert.IsTrue( false, "Invalid game state: " + m_CurrentGameState );
			break;
		}
		}

		GameState backup = m_CurrentGameState;
		m_CurrentGameState = state;

		NotifyStateChange( backup, m_CurrentGameState );
	}
}
