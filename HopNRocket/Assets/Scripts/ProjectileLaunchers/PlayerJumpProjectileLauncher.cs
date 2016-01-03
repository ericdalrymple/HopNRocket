using UnityEngine;
using System.Collections;

public class PlayerJumpProjectileLauncher
: DefaultProjectileLauncher
{
	void OnPlayerJump()
	{
		LaunchProjectile();
	}
}
