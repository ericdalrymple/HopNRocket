using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class TurretShootBehaviour
: StateMachineBehaviour
{
	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		Transform turretAnimatorParent = animator.transform.parent;
		if( null != turretAnimatorParent )
		{
			turretAnimatorParent.gameObject.BroadcastMessage( AbstractProjectileLauncher.MESSAGE_LAUNCH_PROJECTILE
						                                    , AbstractProjectileLauncher.NULL_LAUNCHER_ID
						                                    , SendMessageOptions.DontRequireReceiver );
		}
	}
}
