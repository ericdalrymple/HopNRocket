using UnityEngine;
using System.Collections;

public abstract class Projectile
: MonoBehaviour
{
	protected abstract void OnProjectileLaunch( ProjectileLauncher.ProjectileLaunchEvent eventInfo );

	void Start()
	{
	
	}

	void Update()
	{
	
	}
}
