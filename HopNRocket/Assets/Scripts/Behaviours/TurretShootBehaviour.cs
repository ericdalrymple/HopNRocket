using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class TurretShootBehaviour
: StateMachineBehaviour
{
	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		animator.gameObject.BroadcastMessage( "LaunchProjectile", SendMessageOptions.DontRequireReceiver );
	}
}
