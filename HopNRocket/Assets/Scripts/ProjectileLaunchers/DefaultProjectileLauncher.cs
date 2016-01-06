using UnityEngine;
using System.Collections;

public class DefaultProjectileLauncher
: AbstractProjectileLauncher
{
	public float m_LaunchForce;

	private Rigidbody2D m_Body;

	void Start()
	{
		m_Body = GetComponent<Rigidbody2D>();
	}

	protected void ConfigureDefaultLaunchEvent( ProjectileLaunchEvent launchEvent )
	{
		//-- Does nothing by default; meant to be overridden by subclasses
		//   if they need the launch event to diverge from default settings
		//   while still benefiting from the generation of a default launch
		//   event.
	}

	protected sealed override ProjectileLaunchEvent GenerateLaunchEvent()
	{
		//-- Resolve the direction of the shot relavitve to the world
		Vector3 direction = gameObject.transform.TransformVector( new Vector3( 1.0f, 0.0f, 0.0f ) );

		float resultantForce = m_LaunchForce;
		Vector2 resultantDirection = direction.normalized;	//-- Normalized in case the transform scale changed the length of the vector

		//-- Add the velocity of the projectile launcher to acheive a
		//   nice smooth-looking launch
		if( null != m_Body )
		{
			//-- Get the velocities of the launcher and the launch
			Vector2 bodyVelocity = m_Body.velocity;
			Vector2 launchVelocity = direction * m_LaunchForce;

			//-- Decompose the resultant velocity to get the new force and direction
			Vector2 resultantVelocity = bodyVelocity + launchVelocity;
			resultantForce = resultantVelocity.magnitude;
			resultantDirection = resultantVelocity / resultantForce;
		}

		//-- Generate the launch event
		ProjectileLaunchEvent defaultEventInfo = new ProjectileLaunchEvent();
		{
			defaultEventInfo.launchForce = resultantForce;
			defaultEventInfo.launchDirection = new Vector2( resultantDirection.x, resultantDirection.y );
			defaultEventInfo.gravityScale = Mathf.Sign( ResolveLauncherScaleY() );
		}

		//-- Give the subclass (if any) a chance to modify the default launch event
		ConfigureDefaultLaunchEvent( defaultEventInfo );

		return defaultEventInfo;
	}

	float ResolveLauncherScaleY()
	{
		float scaleY = 1.0f;

		Transform xform = transform;
		while( null != xform )
		{
			scaleY *= xform.localScale.y;
			xform = xform.parent;
		}

		return scaleY;
	}
}
