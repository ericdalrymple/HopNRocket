using UnityEngine;
using System.Collections;

/**
 * All projectiles in the game should inherit from this class.
 * Technically, any MonoBehaviour that responds to an
 * 'OnProjectileLaunch' message can be used as a projectile,
 * but inheriting from this class unburdens the developer from
 * having to know this.
 * 
 * Although this class could be an interface instead, this
 * structure allows for general projectile functionality to be
 * added later (if needed) without needing to refactor a large
 * amount of code.
 */
public abstract class Projectile
: MonoBehaviour
{
	/** Message-based callback informing the projectile of its launch context. */
	protected abstract void OnProjectileLaunch( ProjectileLauncher.ProjectileLaunchEvent eventInfo );
}
