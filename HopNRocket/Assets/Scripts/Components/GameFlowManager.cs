using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class GameFlowManager
: MonoBehaviour
{
	//-- States for the game's overall state machine
	private enum GameState
	{
		  NONE
		, TITLE_SCREEN
		, PLAYING
		, GAME_OVER
	}

	//-- Messages sent by this MonoBehaviour
	private static readonly string MESSAGE_GAME_PLAYING = "OnGamePlaying";

	//-- Member variables
	public GameObject m_TitleScreen;
	public GameObject m_GameOverScreen;
	public GameObject m_HUD;

	private GameObject m_PlayerObject;
	private GameState m_CurrentGameState;

	void Start()
	{
		//-- Initialize game state to "NONE"
		m_CurrentGameState = GameState.NONE;

		//-- Cache player object
		m_PlayerObject = GameObject.FindGameObjectWithTag( "Player" );

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
			if( null != m_PlayerObject )
			{
				//-- Let the player know the game started
				m_PlayerObject.SendMessage( MESSAGE_GAME_PLAYING
				                          , SendMessageOptions.DontRequireReceiver );
			}

			gameObject.SendMessage( MESSAGE_GAME_PLAYING
			                      , SendMessageOptions.DontRequireReceiver );

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

		m_CurrentGameState = state;

		//-- Update the game UI to match the new game state
		UpdateGameUI();
	}

	void UpdateGameUI()
	{
		switch( m_CurrentGameState )
		{
			case GameState.TITLE_SCREEN:
			{
				if( null != m_TitleScreen )
				{
					m_TitleScreen.SetActive( true );
				}
				
				if( null != m_GameOverScreen )
				{
					m_GameOverScreen.SetActive( false );
				}
				
				if( null != m_HUD )
				{
					m_HUD.SetActive( false );
				}
				
				break;
			}
				
			case GameState.PLAYING:
			{
				if( null != m_TitleScreen )
				{
					m_TitleScreen.SetActive( false );
				}
				
				if( null != m_GameOverScreen )
				{
					m_GameOverScreen.SetActive( false );
				}
				
				if( null != m_HUD )
				{
					m_HUD.SetActive( true );
				}
				
				break;
			}
				
			case GameState.GAME_OVER:
			{
				if( null != m_TitleScreen )
				{
					m_TitleScreen.SetActive( false );
				}
				
				if( null != m_GameOverScreen )
				{
					m_GameOverScreen.SetActive( true );
				}
				
				if( null != m_HUD )
				{
					m_HUD.SetActive( false );
				}
				
				break;
			}
		}
	}
}
