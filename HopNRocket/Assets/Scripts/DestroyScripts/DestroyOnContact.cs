using UnityEngine;
using System.Collections;

public class DestroyOnContact
: MonoBehaviour
{
	void OnCollisionEnter2D( Collision2D collision )
	{
		//-- Destroy this game object if it bumps into something solid that isn't its parent
		if( !collision.collider.isTrigger && !IsParent( collision.gameObject ) )
		{
			Destroy( gameObject );
		}
	}

	void OnTriggerEnter2D( Collider2D collider )
	{
		//-- Destroy this game object if it bumps into something solid that isn't its parent
		if( !collider.isTrigger && !IsParent( collider.gameObject ) )
		{
			Destroy( gameObject );
		}
	}

	bool IsParent( GameObject otherGameObject )
	{
		Transform xform = gameObject.transform;
		while( null != xform )
		{
			if( xform == otherGameObject.transform )
			{
				return true;
			}

			xform = xform.parent;
		}

		return false;
	}
}
