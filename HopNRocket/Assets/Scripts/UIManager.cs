using UnityEngine;
using System.Collections;

public class UIManager
: MonoBehaviour
{
	public GameObject m_TitleScreen;
	public GameObject m_GameOverScreen;
	public GameObject m_HUD;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void OnGameStateChange( GameController.GameStateEvent eventInfo )
	{
		UpdateGameUI( eventInfo.currentState );
	}

	void UpdateGameUI( GameController.GameState gameState )
	{
		switch( gameState )
		{
			case GameController.GameState.TITLE_SCREEN:
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
				
			case GameController.GameState.PLAYING:
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
				
			case GameController.GameState.GAME_OVER:
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
