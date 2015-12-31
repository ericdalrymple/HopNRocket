using UnityEngine;
using System.Collections;

public class WorldScroller
: MonoBehaviour
{
	private static readonly string SCROLLABLE_TAG = "Scrollable";

	public float m_InitialScrollSpeed;
	public float m_ScrollAcceleration;

	private float m_CurrentScrollSpeed;

	void Start()
	{
		m_CurrentScrollSpeed = m_InitialScrollSpeed;
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
}
