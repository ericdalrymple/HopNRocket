using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class TogglePlayerInputBehaviour
: StateMachineBehaviour
{
	public enum PlayerInputAction
	{
		  ENABLE = 0
		, DISABLE
	}

	public bool enable{ get{ return (m_Input == PlayerInputAction.ENABLE); } }

	public PlayerInputAction m_Input;
	public bool m_OnStateEnter = true;
	public bool m_OnStateExit = false;

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		if( m_OnStateEnter )
		{
			animator.gameObject.SendMessage( PlayerController.MESSAGE_TOGGLE_PLAYER_INPUT
			                               , this.enable
			                               , SendMessageOptions.RequireReceiver );
		}
	}

	public override void OnStateExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		if( m_OnStateExit )
		{
			animator.gameObject.SendMessage( PlayerController.MESSAGE_TOGGLE_PLAYER_INPUT
			                               , this.enable
			                               , SendMessageOptions.RequireReceiver );
		}
	}
}
