using UnityEngine;
using System.Collections;

public class WorldScroller
: GameControllerSystem<WorldScroller>
{
	//-- Settings
	[Tooltip( "Scroll speed, in units per second, at which the game initially scrolls" )]
	public float m_InitialScrollSpeed;

	[Tooltip( "Rate, in units per second per second, at which the game's scroll speed " +
			  "increases over time" )]
	public float m_ScrollAcceleration;

	[Tooltip( "Discrete amount, in units per second, by which the game's scroll speed " +
		      "changes when using WorldScroller's increment/decrement methods" )]
	public float m_ScrollSpeedIncrement;


	//-- Attributes
	public float relativeScrollSpeed{ get{ return m_CurrentScrollSpeed / m_InitialScrollSpeed; } }
	public float scrollSpeed{ get{ return m_CurrentScrollSpeed; } }


	//-- Members
	private float m_CurrentScrollSpeed;

	void Start()
	{
		m_CurrentScrollSpeed = 0.0f;
	}

	void Update()
	{
		UpdateScrollSpeed();
	}

	void UpdateScrollSpeed()
	{
		//-- Apply acceleration to the current scroll speed
		m_CurrentScrollSpeed += m_ScrollAcceleration * Time.deltaTime;
	}

	void OnGameStateChange( GameController.GameStateEvent eventInfo )
	{
		switch( eventInfo.currentState )
		{
			case GameController.GameState.GAME_OVER:
			case GameController.GameState.TITLE_SCREEN:
			{
				//-- Don't scroll on title or game over
				m_CurrentScrollSpeed = 0.0f;
				break;
			}

			case GameController.GameState.PLAYING:
			{
				//-- Reset scrolling on playing
				ResetScrollSpeed();
				break;
			}
		}
	}

	public void DecrementScrollSpeed()
	{
		//-- Slow down the game
		m_CurrentScrollSpeed -= m_ScrollSpeedIncrement;

		//-- Can't slow down more than the initial scroll speed
		m_CurrentScrollSpeed = Mathf.Max( m_InitialScrollSpeed, m_CurrentScrollSpeed );
	}

	public void IncrementScrollSpeed()
	{
		//-- Speed up the game
		m_CurrentScrollSpeed += m_ScrollSpeedIncrement;

		//-- Can't slow down more than the initial scroll speed (handles negative scroll speed increments)
		m_CurrentScrollSpeed = Mathf.Max( m_InitialScrollSpeed, m_CurrentScrollSpeed );
	}

	public void ResetScrollSpeed()
	{
		m_CurrentScrollSpeed = m_InitialScrollSpeed;
	}
}
