using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializer
: MonoBehaviour
{
    public GameObject[] m_GroundTilePrefabs;
    
    private BoxCollider2D m_GameArea;
	private Vector2 m_ViewableAreaSize;

	void Start()
    {
		//-- Compute the viewable area in game space
		float viewableAreaHeight = Camera.main.orthographicSize * 2.0f;
		float viewableAreaWidth = viewableAreaHeight * Screen.width / Screen.height;
		m_ViewableAreaSize = new Vector2( viewableAreaWidth
		                                , viewableAreaHeight );

		InitializeCeiling();
		InitializeGameArea();

		//-- Generate ground tiles
		GroundManager.Instance.GenerateGround( m_GameArea.bounds.min.x
		                                     , m_GameArea.bounds.max.x
		                                     , m_GameArea.bounds.min.y
		                                     , m_GroundTilePrefabs );
	}
	
	void Update()
    {

	}

	void InitializeCeiling()
	{
		//-- Get a reference to the ceiling object
		GameObject ceilingObject = GameObject.FindGameObjectWithTag( "Ceiling" );
		if( null == ceilingObject )
		{
			//-- No ceiling; can't do anything
			return;
		}

		//-- Position the ceiling at the top of the game area
		ceilingObject.transform.position = new Vector3( 0.0f, m_ViewableAreaSize.y * 0.5f, 0.0f );

		//-- Get a reference to the collider
		BoxCollider2D ceilingCollider = ceilingObject.GetComponent<BoxCollider2D>();
		if( null != ceilingCollider )
		{
			//-- Resize the ceiling to the width of the viewable area
			ceilingCollider.size = new Vector2( m_ViewableAreaSize.x, ceilingCollider.size.y );
		}
	}

	void InitializeGameArea()
	{
		//-- Get a reference to the game area
		m_GameArea = gameObject.GetComponent<BoxCollider2D>();

		//-- Resize the game area to the visible area
		if( null != m_GameArea )
		{
			m_GameArea.size = new Vector2( m_ViewableAreaSize.x, m_ViewableAreaSize.y );
		}
	}
}
