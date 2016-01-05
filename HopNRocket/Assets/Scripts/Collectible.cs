using UnityEngine;
using System.Collections;

public class Collectible
: MonoBehaviour
{
	void OnTriggerEnter2D( Collider2D collider )
	{
		if( collider.gameObject.CompareTag( "Player" ) )
		{
			InventoryManager.instance.AddItems( 1 );
			Destroy( gameObject );
		}
	}
}
