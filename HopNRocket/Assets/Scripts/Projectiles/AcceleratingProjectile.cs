using UnityEngine;
using System.Collections;

public class AcceleratingProjectile
: Projectile
{
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
			Vector2 acceleration = m_Body.velocity.normalized * m_Acceleration * Time.deltaTime;
			m_Body.velocity += acceleration;
		}
	}

	protected override void OnProjectileLaunch( ProjectileLauncher.ProjectileLaunchEvent eventInfo )
	{
		if( null != m_Body )
		{
			m_Body.velocity = eventInfo.launchDirection * eventInfo.launchForce;
		}
	}
}
