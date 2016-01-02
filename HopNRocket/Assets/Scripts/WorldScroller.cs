using UnityEngine;
using System.Collections;

public class WorldScroller
: MonoBehaviour
{
	private static readonly string SCROLLABLE_TAG = "Scrollable";

	public float m_InitialScrollSpeed;
	public float m_ScrollAcceleration;
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

	void OnTriggerExit2D( Collider2D collider )
	{
		GameObject colliderObject = collider.gameObject;
		if( colliderObject.CompareTag( SCROLLABLE_TAG ) )
		{
			colliderObject.SendMessage( "OnScrollableExit" );
		}
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

	void OnPlayerJump()
	{
		//-- Speed up the game on every hop
		m_CurrentScrollSpeed += m_ScrollSpeedIncrement;
	}

	void OnPlayerShot()
	{
		//-- Slow down the game on every shot
		m_CurrentScrollSpeed -= m_ScrollSpeedIncrement;

		//-- Can't slow down more than the initial scroll speed
		m_CurrentScrollSpeed = Mathf.Max( m_InitialScrollSpeed, m_CurrentScrollSpeed );
	}
}
