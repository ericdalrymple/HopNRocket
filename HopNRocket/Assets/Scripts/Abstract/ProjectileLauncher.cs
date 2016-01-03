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
	private static readonly string MESSAGE_PROJECTILE_LAUNCHED = "OnProjectileLaunch";

	//-- Member variables
	public GameObject m_Projectile;

	//-- Abstract methods & functions
	protected abstract ProjectileLaunchEvent GenerateLaunchEvent();

	protected void LaunchProjectile()
	{
		GameObject projectileInstance = Instantiate( m_Projectile
										           , gameObject.transform.position
										           , Quaternion.identity ) as GameObject;

		projectileInstance.transform.SetParent( gameObject.transform );

		ProjectileLaunchEvent launchEvent = GenerateLaunchEvent();

		projectileInstance.SendMessage( MESSAGE_PROJECTILE_LAUNCHED
				                      , launchEvent
				                      , SendMessageOptions.DontRequireReceiver );
	}
}
