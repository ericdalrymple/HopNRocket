using UnityEngine;
using System.Collections;

public class LoopOnScrollableExit
: MonoBehaviour
{
	void OnScrollableExit()
	{
		GroundManager.Instance.ReEnqueue( gameObject );
	}
}
