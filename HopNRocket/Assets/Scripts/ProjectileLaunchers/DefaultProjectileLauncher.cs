using UnityEngine;
using System.Collections;

public class DefaultProjectileLauncher
: ProjectileLauncher
{
	public float m_LaunchForce;

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
		Transform objectTransform = gameObject.transform;
		Vector3 direction = new Vector3( 1.0f, 0.0f, 0.0f );
		while( null != objectTransform )
		{
			//-- Transform the direction by the transform
			direction = objectTransform.TransformDirection( direction );

			//-- Get the parent game object
			objectTransform = objectTransform.parent;
		}

		//-- Generate the launch event
		ProjectileLaunchEvent defaultEventInfo = new ProjectileLaunchEvent();
		{
			defaultEventInfo.launchForce = m_LaunchForce;
			defaultEventInfo.launchDirection = new Vector2( direction.x, direction.y );
		}

		//-- Give the subclass (if any) a chance to modify the default launch event
		ConfigureDefaultLaunchEvent( defaultEventInfo );

		return defaultEventInfo;
	}
}
