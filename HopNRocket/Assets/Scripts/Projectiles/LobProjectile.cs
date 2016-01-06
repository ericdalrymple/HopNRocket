using UnityEngine;
using System.Collections;

/**
 * A projectile with no self-propulsion that obeys gravity.
 * It's initial velocity is determined by the launch events
 * provided by the projectile launcher that produced it.
 */
public class LobProjectile
: AbstractProjectile
{
	private Rigidbody2D m_Body;

	void Awake()
	{
		m_Body = GetComponent<Rigidbody2D>();
	}

	protected override void OnProjectileLaunch( AbstractProjectileLauncher.ProjectileLaunchEvent eventInfo )
	{
		if( null != m_Body )
		{
			//-- Push the projectile from its current position in the launch direction
			m_Body.AddForce( eventInfo.launchDirection * eventInfo.launchForce
			               , ForceMode2D.Impulse );

			//-- Incorporate the launcher's gravity scale
			m_Body.gravityScale *= eventInfo.gravityScale;
		}
	}
}
