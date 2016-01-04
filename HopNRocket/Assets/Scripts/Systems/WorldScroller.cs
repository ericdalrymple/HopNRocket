using UnityEngine;
using System.Collections;

public class WorldScroller
: MonoBehaviour
{
	//-- Constants
	private static readonly string SCROLLABLE_TAG = "Scrollable";

	//-- Memeber variables
	[Tooltip( "Scroll speed, in units per second, at which the game initially scrolls" )]
	public float m_InitialScrollSpeed;

	[Tooltip( "Rate, in units per second per second, at which the game's scroll speed " +
			  "increases over time" )]
	public float m_ScrollAcceleration;

	[Tooltip( "Discrete amount, in units per second, by which the game's scroll speed " +
		      "changes when using WorldScroller's increment/decrement methods" )]
	public float m_ScrollSpeedIncrement;

	private float m_CurrentScrollSpeed;

	void Start()
	{
		m_CurrentScrollSpeed = 0.0f;
	}

	void Update()
	{
		UpdateScrollables();
		UpdateScrollSpeed();
	}

	void UpdateScrollables()
	{
		//-- Get all of the scrollable game objects
		GameObject[] scrollables = GameObject.FindGameObjectsWithTag( SCROLLABLE_TAG );

		//-- Update the position of all the scrollables according to the current scroll speed
		foreach( GameObject scrollable in scrollables )
		{
			scrollable.transform.Translate( -m_CurrentScrollSpeed * Time.deltaTime, 0.0f, 0.0f );
		}
	}

	void UpdateScrollSpeed()
	{
		//-- Apply acceleration to the current scroll speed
		m_CurrentScrollSpeed += m_ScrollAcceleration * Time.deltaTime;
	}

	void OnGameStateChange( GameController.GameStateEvent eventInfo )
	{
		if( GameController.GameState.GAME_OVER == eventInfo.currentState )
		{
			m_CurrentScrollSpeed = 0.0f;
		}
		else if( GameController.GameState.PLAYING == eventInfo.currentState )
		{
			m_CurrentScrollSpeed = m_InitialScrollSpeed;
		}
	}

	void DecrementScrollSpeed()
	{
		//-- Slow down the game
		m_CurrentScrollSpeed -= m_ScrollSpeedIncrement;

		//-- Can't slow down more than the initial scroll speed
		m_CurrentScrollSpeed = Mathf.Max( m_InitialScrollSpeed, m_CurrentScrollSpeed );
	}

	void IncrementScroolSpeed()
	{
		//-- Speed up the game
		m_CurrentScrollSpeed += m_ScrollSpeedIncrement;

		//-- Can't slow down more than the initial scroll speed (handles negative scroll speed increments)
		m_CurrentScrollSpeed = Mathf.Max( m_InitialScrollSpeed, m_CurrentScrollSpeed );
	}
}
