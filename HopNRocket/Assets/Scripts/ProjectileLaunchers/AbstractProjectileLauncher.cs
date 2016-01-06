using UnityEngine;
using System.Collections;

/**
 * Provides a means to generate and launch projectiles, but has
 * no trigger conditions for doing so. Subclasses of
 * ProjectileLauncher are expected to provide the trigger
 * conditions and call the LaunchProjectile() function when
 * those trigger conditions are met.
 */
public abstract class AbstractProjectileLauncher
: MonoBehaviour
{
	//-- Internal definitions

	/**
	 * Struct defining the circumstances of each instance in
	 * which this projectile launcher launches a projectile. Each
	 * projectile is given an instance of this struct through
	 * a message when it is created in order to configure itself.
	 */
	public class ProjectileLaunchEvent
	{
		/** Force with which the projectile is being expulsed. */
		public float launchForce = 0.0f;

		/** Direction in which the projectile is being expulsed. */
		public Vector2 launchDirection = Vector2.right;

		/** Gravity scale proposed to the projectile by the launcher */
		public float gravityScale = 1.0f;
	}


	//-- Messages supported by this MonoBehaviour

	/** Fires a projectile */
	public static readonly string MESSAGE_LAUNCH_PROJECTILE = "OnLaunchProjectile";


	//-- Constants
	public static readonly string NULL_LAUNCHER_ID = "";

	//-- Settings

	[Tooltip( "Unique ID to identify this launcher within its object hierarchy." )]
	public string m_LauncherId = NULL_LAUNCHER_ID;

	[Tooltip( "Type of projectile launched by this projectile launcher.")]
	public GameObject m_ProjectilePrefab;


	//-- Abstract methods & functions

	/**
	 * Hook for discrete subclasses to produce the launch parameters for their projectiles.
	 */
	protected abstract ProjectileLaunchEvent GenerateLaunchEvent();


	//-- Message handlers

	void OnLaunchProjectile( string launcherId )
	{
		if( launcherId.Equals( m_LauncherId ) )
		{
			LaunchProjectile();
		}
	}


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

		//-- Set projectile collection as parent
		projectileInstance.transform.SetParent( ProjectileCollection.instance.gameObject.transform );

		//-- Generate and send launch parameters to the new projectile
		ProjectileLaunchEvent launchEvent = GenerateLaunchEvent();
		projectileInstance.SendMessage( AbstractProjectile.MESSAGE_LAUNCH
				                      , launchEvent
				                      , SendMessageOptions.RequireReceiver );
	}
}
