using UnityEngine;
using System.Collections;

public class DamageOnCollision
: LayerSelector
{
	void OnCollisionEnter2D( Collision2D collision )
	{
		if( IsOnTargetLayer( collision.gameObject ) )
		{
			SendHit( collision.gameObject );
		}
	}

	void SendHit( GameObject gameObject )
	{
		gameObject.BroadcastMessage( Killable.MESSAGE_HIT, SendMessageOptions.DontRequireReceiver );
	}
}
