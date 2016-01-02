using UnityEngine;
using System.Collections;

public class DestroyOnContact
: MonoBehaviour
{
	void OnCollisionEnter2D( Collision2D collision )
	{
		//-- Destroy this game object if it bumps into something solid
		if( !collision.collider.isTrigger )
		{
			Destroy( gameObject );
		}
	}

	void OnTriggerEnter2D( Collider2D collider )
	{
		//-- Destroy this game object if it bumps into something solid
		if( !collider.isTrigger )
		{
			Destroy( gameObject );
		}
	}
}
