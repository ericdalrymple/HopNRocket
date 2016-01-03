using UnityEngine;
using System.Collections;

public class LobProjectile
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
			m_Body.AddForce( eventInfo.launchDirection * eventInfo.launchForce
			               , ForceMode2D.Impulse );
		}
	}
}
