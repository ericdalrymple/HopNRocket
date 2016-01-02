using UnityEngine;
using System.Collections;

public class GroundTile
: MonoBehaviour
{
	void OnScrollableExit()
	{
		GroundManager groundManager = GroundManager.Instance;
		if( null != groundManager )
		{
			groundManager.ReEnqueue( gameObject );
		}
	}
}
