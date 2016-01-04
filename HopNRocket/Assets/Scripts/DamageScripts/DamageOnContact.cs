using UnityEngine;
using System.Collections;

public class DamageOnContact
: LayerSelector
{
	private static readonly string MESSAGE_HIT = "OnHit";

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
		gameObject.BroadcastMessage( MESSAGE_HIT, SendMessageOptions.DontRequireReceiver );
	}
}
