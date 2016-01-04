using UnityEngine;
using System.Collections;

/**
 * A projectile that travels in a straight line at a contant
 * rate. It's initial velocity is determined by the launch
 * events provided by the projectile launcher that produced it.
 */
public class BasicProjectile
: Projectile
{
	private Rigidbody2D m_Body;

	void Awake()
	{
		m_Body = GetComponent<Rigidbody2D>();
	}

	protected override void OnProjectileLaunch( ProjectileLauncher.ProjectileLaunchEvent eventInfo )
	{
		if( null != m_Body )
		{
			//-- Apply launch velocity
			m_Body.velocity = eventInfo.launchDirection * eventInfo.launchForce;
		}
	}
}
