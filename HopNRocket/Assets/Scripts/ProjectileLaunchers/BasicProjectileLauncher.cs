using UnityEngine;
using System.Collections;

public class BasicProjectileLauncher
: DefaultProjectileLauncher
{
	public float m_LaunchDelay;
	public float m_LaunchInterval;

	void Start()
	{
		StartCoroutine( Launch() );
	}

	IEnumerator Launch()
	{
		yield return new WaitForSeconds( m_LaunchDelay );

		while( true )
		{
			base.LaunchProjectile();

			yield return new WaitForSeconds( m_LaunchInterval );
		}
	}
}
