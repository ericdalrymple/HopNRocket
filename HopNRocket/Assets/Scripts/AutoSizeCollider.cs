using UnityEngine;
using System.Collections;

public class AutoSizeCollider
: MonoBehaviour
{
	public float m_ScreenHeightRatio;

	private CircleCollider2D m_Collider;

	void Start()
	{
		GameObject gameAreaObject = GameObject.FindGameObjectWithTag( "GameArea" );
		if( null != gameAreaObject )
		{
			BoxCollider2D gameArea = gameAreaObject.GetComponent<BoxCollider2D>();

			m_Collider = GetComponent<CircleCollider2D>();
			if( null != m_Collider )
			{
				m_Collider.radius = gameArea.size.y * m_ScreenHeightRatio;
			}
		}
	}
}
