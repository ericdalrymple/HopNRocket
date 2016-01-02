using UnityEngine;
using System.Collections;

public class AutoSizeTurretSensor
: MonoBehaviour
{
	public float m_ScreenHeightRatio;
	public BoxCollider2D m_GameArea;

	private CircleCollider2D m_Collider;

	void Start()
	{
		if( null != m_GameArea )
		{
			m_Collider = GetComponent<CircleCollider2D>();
			if( null != m_Collider )
			{
				m_Collider.radius = m_GameArea.size.y * m_ScreenHeightRatio;
			}
		}
	}

	void OnTriggerEnter2D( Collider2D collider )
	{

	}
}
