using UnityEngine;
using System.Collections;

public class PlayerShootProjectileLauncher
: DefaultProjectileLauncher
{
	void OnPlayerShot()
	{
		LaunchProjectile();
	}
}
