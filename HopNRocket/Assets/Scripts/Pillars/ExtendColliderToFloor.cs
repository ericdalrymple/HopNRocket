using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class ExtendColliderToFloor
: MonoBehaviour
{
	private BoxCollider2D m_Collider;
	
	void Start()
	{
		m_Collider = GetComponent<BoxCollider2D>();
		
		GameObject gameAreaObject = GameObject.FindGameObjectWithTag( "GameArea" );
		if( null != gameAreaObject )
		{
			BoxCollider2D gameArea = gameAreaObject.GetComponent<BoxCollider2D>();
			if( null != gameArea )
			{
				ExtendCollider( gameArea.bounds.min.y );
			}
		}
	}

	void ExtendCollider( float floorY )
	{
		float currentTop = m_Collider.bounds.max.y;
		float oldHeight = m_Collider.size.y;
		float newHeight = m_Collider.bounds.max.y - floorY;
		float pushDown = (newHeight - oldHeight) * 0.5f;

		m_Collider.offset += new Vector2( 0.0f, -pushDown );
		m_Collider.size = new Vector2( m_Collider.size.x, newHeight );
	}
}
