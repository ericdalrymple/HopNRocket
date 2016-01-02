using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class FitToCeiling
: MonoBehaviour
{
	public BoxCollider2D m_GameArea;

	private BoxCollider2D m_CeilingArea;

	void Start()
	{
		m_CeilingArea = GetComponent<BoxCollider2D>();
		if( null == m_CeilingArea )
		{
			Assert.IsTrue( false, "Game objects using the FitToCeiling script must also have a BoxCollider2D component." );
			return;
		}

		InitializeCeiling();
	}

	/**
	 * Positions the ceiling game object above the play area such that
	 * the player character will collide against the top of the visible
	 * game area. Also resizes the ceiling collider such that it spans
	 * the width of the visible game area.
	 */
	void InitializeCeiling()
	{
		if( null == m_CeilingArea )
		{
			//-- No ceiling; can't do anything
			return;
		}
		
		//-- Compute the y-position of the ceiling collider's bottom relative to its game object
		float colliderBottomY = m_CeilingArea.offset.y - m_CeilingArea.size.y * 0.5f;
		
		//-- Position the ceiling at the top of the game area
		gameObject.transform.position = new Vector3( 0.0f
		                                           , m_GameArea.size.y * 0.5f - colliderBottomY
		                                           , 0.0f );
		
		//-- Resize the ceiling to the width of the viewable area
		m_CeilingArea.size = new Vector2( m_GameArea.size.x, m_CeilingArea.size.y );
	}
}
