using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class ConsumeSpecialBehaviour
: StateMachineBehaviour
{
	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		bool consumed = InventoryManager.instance.ConsumeItem();
		if( consumed )
		{
			//TODO Perform special ability
		}
	}
}
