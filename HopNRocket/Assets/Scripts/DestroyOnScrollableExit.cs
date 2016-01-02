using UnityEngine;
using System.Collections;

public class DestroyOnScrollableExit
: MonoBehaviour
{
	void OnScrollableExit()
	{
		Destroy( gameObject );
	}
}
