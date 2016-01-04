using UnityEngine;
using System.Collections;

/**
 * A projectile that travels in a straight line with a specified
 * acceleration. It's initial velocity is determined by the
 * launch events provided by the projectile launcher that
 * produced it.
 */
public class AcceleratingProjectile
: AbstractProjectile
{
	/** Acceleration in game units per second per second */
	public float m_Acceleration;

	private Rigidbody2D m_Body;

	void Awake()
	{
		m_Body = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if( null != m_Body )
		{
			//-- Apply acceleration in the direction of the current velocity
			Vector2 acceleration = m_Body.velocity.normalized * m_Acceleration * Time.deltaTime;
			m_Body.velocity += acceleration;
		}
	}

	protected override void OnProjectileLaunch( AbstractProjectileLauncher.ProjectileLaunchEvent eventInfo )
	{
		if( null != m_Body )
		{
			//-- Apply initial velocity according to launch parameters
			m_Body.velocity = eventInfo.launchDirection * eventInfo.launchForce;
		}
	}
}
