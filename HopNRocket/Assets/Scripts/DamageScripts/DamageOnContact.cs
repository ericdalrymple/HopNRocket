using UnityEngine;
using System.Collections;

public class DamageOnContact
: LayerSelector
{
	void OnCollisionEnter2D( Collision2D collision )
	{
		if( IsOnTargetLayer( collision.gameObject ) )
		{
			SendHit( collision.gameObject );
		}
	}

	void OnTriggerEnter2D( Collider2D collider )
	{
		if( IsOnTargetLayer( collider.gameObject ) )
		{
			SendHit( collider.gameObject );
		}
	}

	void SendHit( GameObject gameObject )
	{
		gameObject.BroadcastMessage( Killable.MESSAGE_HIT, SendMessageOptions.DontRequireReceiver );
	}
}
