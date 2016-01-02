using UnityEngine;
using System.Collections;

public abstract class ProjectileLauncher
: MonoBehaviour
{
	public struct ProjectileLaunchEvent
	{
		public float launchForce;
		public Vector2 launchDirection;
	}

	//-- Messages sent by this MonoBehaviour
	private static readonly string MESSAGE_PROJECTILE_LAUNCHED = "OnProjectileLaunched";

	//-- Abstract methods & functions
	protected abstract ProjectileLaunchEvent GenerateLaunchEvent();

	protected void LaunchProjectile( GameObject projectile )
	{
		GameObject projectileInstance = Instantiate( projectile
										           , gameObject.transform.position
										           , Quaternion.identity ) as GameObject;

		ProjectileLaunchEvent launchEvent = GenerateLaunchEvent();

		projectileInstance.SendMessage( MESSAGE_PROJECTILE_LAUNCHED
				                      , launchEvent
				                      , SendMessageOptions.DontRequireReceiver );
	}
}
