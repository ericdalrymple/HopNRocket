using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class FitToVisibleArea
: MonoBehaviour
{
	private BoxCollider2D m_Area;
	private Vector2 m_VisibleAreaSize;

	void Start()
	{
		m_Area = GetComponent<BoxCollider2D>();
		if( null == m_Area )
		{
			Assert.IsTrue( false, "Game objects using the FitToVisible script must also have a BoxCollider2D component." );
			return;
		}

		InitializeGameArea();
	}

	void InitializeGameArea()
	{
		//-- Compute the viewable area in game space
		float viewableAreaHeight = Camera.main.orthographicSize * 2.0f;
		float viewableAreaWidth = viewableAreaHeight * Screen.width / Screen.height;
		m_VisibleAreaSize = new Vector2( viewableAreaWidth
		                                , viewableAreaHeight );
		
		//-- Resize the game area to the visible area
		if( null != m_Area )
		{
			m_Area.transform.position = Vector3.zero;
			m_Area.transform.rotation = Quaternion.identity;
			m_Area.size = new Vector2( m_VisibleAreaSize.x, m_VisibleAreaSize.y );
		}
	}
}
