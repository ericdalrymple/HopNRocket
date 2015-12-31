using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInitializer
: MonoBehaviour
{
    public GameObject[] m_GroundTilePrefabs;
    
    private BoxCollider2D m_GameArea;
    private LinkedList<GameObject> m_GroundTiles = new LinkedList<GameObject>();

	void Start()
    {
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

	void InitializeGameArea()
	{
		//-- Get a reference to the game area
		m_GameArea = gameObject.GetComponent<BoxCollider2D>();

		//-- Resize the game area to the visible area
		if( null != m_GameArea )
		{
			float screenHeight = Camera.main.orthographicSize * 2.0f;
			float screenWidth = screenHeight * Screen.width / Screen.height;

			m_GameArea.size = new Vector2( screenWidth, screenHeight );
		}
	}
}
