using UnityEngine;
using System.Collections;

/**
 * Provides a means to generate and launch projectiles, but has
 * no trigger conditions for doing so. Subclasses of
 * ProjectileLauncher are expected to provide the trigger
 * conditions and call the LaunchProjectile() function when
 * those trigger conditions are met.
 */
public abstract class ProjectileLauncher
: MonoBehaviour
{
	//-- Internal definitions

	/**
	 * Struct defining the circumstances of each instance in
	 * which this projectile launcher launches a projectile. Each
	 * projectile is given an instance of this struct through
	 * a message when it is created in order to configure itself.
	 */
	public struct ProjectileLaunchEvent
	{
		/** Force with which the projectile is being expulsed. */
		public float launchForce;

		/** Direction in which the projectile is being expulsed. */
		public Vector2 launchDirection;
	}


	//-- Messages sent by this MonoBehaviour

	/** Message sent to every projectile launched by this projectile launcher upon creation. */
	private static readonly string MESSAGE_PROJECTILE_LAUNCHED = "OnProjectileLaunch";


	//-- Member variables

	/** Type of projectile launched by this projectile launcher. */
	public GameObject m_ProjectilePrefab;


	//-- Abstract methods & functions

	/**
	 * Hook for discrete subclasses to produce the launch parameters for their projectiles.
	 */
	protected abstract ProjectileLaunchEvent GenerateLaunchEvent();


	//-- Class body

	/**
	 * Instantiates and launches an instance of the projectile prefab assigned to this projectile launcher.
	 */
	protected void LaunchProjectile()
	{
		//-- Make the projectile
		GameObject projectileInstance = Instantiate( m_ProjectilePrefab
										           , gameObject.transform.position
										           , Quaternion.identity ) as GameObject;

		//-- Set this projectile launcher's game object as the new projectile's parent
		projectileInstance.transform.SetParent( gameObject.transform );

		//-- Generate and send launch parameters to the new projectile
		ProjectileLaunchEvent launchEvent = GenerateLaunchEvent();
		projectileInstance.SendMessage( MESSAGE_PROJECTILE_LAUNCHED
				                      , launchEvent
				                      , SendMessageOptions.RequireReceiver );
	}
}
