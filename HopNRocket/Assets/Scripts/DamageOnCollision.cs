using UnityEngine;
using System.Collections;

public class DamageOnCollision
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

	void SendHit( GameObject gameObject )
	{
		gameObject.BroadcastMessage( MESSAGE_HIT, SendMessageOptions.DontRequireReceiver );
	}
}
