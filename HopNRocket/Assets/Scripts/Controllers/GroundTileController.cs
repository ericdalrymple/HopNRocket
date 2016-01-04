using UnityEngine;
using System.Collections;

public class GroundTileController
: MonoBehaviour
{
	void OnTriggerExit2D( Collider2D collider )
	{
		GroundManager groundManager = GroundManager.Instance;
		if( null == groundManager )
		{
			//-- Need the ground manager to loop the tile
			return;
		}

		if( collider.gameObject.CompareTag( "GameArea" ) )
		{
			//-- Loop the tile when it leaves the game area
			groundManager.ReEnqueue( gameObject );
		}
	}
}
