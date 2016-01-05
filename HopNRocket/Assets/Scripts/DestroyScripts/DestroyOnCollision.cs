using UnityEngine;
using System.Collections;

public class DestroyOnCollision
: LayerSelector
{
	public bool m_LayersAsExceptions = true;

	void OnCollisionEnter2D( Collision2D collision )
	{
		if( IsValidContact( collision.gameObject ) )
		{
			Destroy( gameObject );
		}
	}

	void OnTriggerEnter2D( Collider2D collider )
	{
		if( collider.isTrigger )
		{
			//-- No rigid body involved, no collision
			return;
		}

		if( IsValidContact( collider.gameObject ) )
		{
 			Destroy( gameObject );
		}
	}

	bool IsValidContact( GameObject colliderObject )
	{
		bool colliderOnTargetLayer = base.IsOnTargetLayer( colliderObject );
		if( colliderOnTargetLayer )
		{
			if( m_LayersAsExceptions )
			{
				//-- Exempted
				return false;
			}
		}
		else if( !m_LayersAsExceptions )
		{
			//-- Collider not included on target layers
			return false;
		}

		return true;
	}
}
