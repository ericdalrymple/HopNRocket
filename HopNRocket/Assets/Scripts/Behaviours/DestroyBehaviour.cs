using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class DestroyBehaviour
: StateMachineBehaviour
{
	override public void OnStateExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		Destroy( animator.gameObject );
	}
}
