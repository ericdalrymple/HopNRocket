using UnityEngine;
using System.Collections;

public class WorldScrollable
: MonoBehaviour
{
	public float m_ScrollRate = 1.0f;

	void Update()
	{
		//-- Move this object according to the state of the world scroller
		transform.Translate( -WorldScroller.instance.scrollSpeed * m_ScrollRate * Time.deltaTime
		                   , 0.0f, 0.0f );
	}
}
