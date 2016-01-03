using UnityEngine;
using System.Collections;

public class BasicProjectileLauncher
: ProjectileLauncher
{
	public float m_LaunchDelay;
	public float m_LaunchInterval;
	public float m_LaunchForce;

	void Start()
	{
		StartCoroutine( Launch() );
	}

	IEnumerator Launch()
	{
		yield return new WaitForSeconds( m_LaunchDelay );

		while( true )
		{
			base.LaunchProjectile();

			yield return new WaitForSeconds( m_LaunchInterval );
		}
	}

	protected override ProjectileLaunchEvent GenerateLaunchEvent()
	{
		//-- Resolve the direction of the shot relavitve to the world
		Transform objectTransform = gameObject.transform;
		Vector3 direction = new Vector3( 1.0f, 0.0f, 0.0f );
		while( null != objectTransform )
		{
			//-- Transform the direction by the transform
			direction = objectTransform.TransformVector( direction );

			//-- Get the parent game object
			objectTransform = objectTransform.parent;
		}

		//-- Generate the launch event
		ProjectileLaunchEvent eventInfo = new ProjectileLaunchEvent();
		{
			eventInfo.launchForce = m_LaunchForce;
			eventInfo.launchDirection = new Vector2( direction.x, direction.y );
		}

		return eventInfo;
	}
}
