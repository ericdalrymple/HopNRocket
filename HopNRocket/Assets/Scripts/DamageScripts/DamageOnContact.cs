using UnityEngine;
using System.Collections;

public class DamageOnContact
: LayerSelector
{
	public bool m_OnCollision = true;
	public bool m_OnTrigger = true;

	void OnCollisionEnter2D( Collision2D collision )
	{
		if( !m_OnCollision )
		{
			return;
		}

		if( IsOnTargetLayer( collision.gameObject ) )
		{
			SendHit( collision.gameObject );
		}
	}

	void OnTriggerEnter2D( Collider2D collider )
	{
		if( !m_OnTrigger )
		{
			return;
		}

		if( IsOnTargetLayer( collider.gameObject ) )
		{
			SendHit( collider.gameObject );
		}
	}

	void SendHit( GameObject gameObject )
	{
		GameObject messageTarget = gameObject;

		Transform parent = gameObject.transform.parent;
		if( (null != parent) && IsOnTargetLayer( parent.gameObject ) )
		{
			//-- The object that was collided with is most likely a collidable
			//   child of the game object wee need to hit
			messageTarget = parent.gameObject;
		}

		messageTarget.BroadcastMessage( Killable.MESSAGE_HIT, SendMessageOptions.DontRequireReceiver );
	}
}
