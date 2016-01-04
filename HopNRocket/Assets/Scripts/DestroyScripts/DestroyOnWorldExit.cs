using UnityEngine;
using System.Collections;

public class DestroyOnWorldExit
: MonoBehaviour
{
	void OnTriggerExit2D( Collider2D collider )
	{
		if( collider.gameObject.CompareTag( "GameArea" ) )
		{
			Destroy( gameObject );
		}
	}
}
