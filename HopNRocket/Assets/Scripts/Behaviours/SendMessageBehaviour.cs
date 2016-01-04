using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class SendMessageBehaviour
: StateMachineBehaviour
{
	public string m_Message;
	public bool m_Broadcast = false;
	public bool m_SendUpwards = false;
	public bool m_SendOnEnter = true;
	public bool m_SendOnExit = false;
	public bool m_Required = false;

	public string[] m_AdditionalReceiverTags;

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		if( m_SendOnEnter )
		{
			SendBehaviourMessage( animator.gameObject );
		}
	}

	public override void OnStateExit( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		if( m_SendOnExit )
		{
			SendBehaviourMessage( animator.gameObject );
		}
	}

	void SendBehaviourMessage( GameObject gameObject )
	{
		SendBehaviourMessageCore( gameObject );

		foreach( string tag in m_AdditionalReceiverTags )
		{
			GameObject[] receivers = GameObject.FindGameObjectsWithTag( tag );
			foreach( GameObject receiver in receivers )
			{
				SendBehaviourMessageCore( receiver );
			}
		}
	}

	void SendBehaviourMessageCore( GameObject gameObject )
	{
		SendMessageOptions options = m_Required? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver;
		if( m_Broadcast )
		{
			gameObject.BroadcastMessage( m_Message, options );
		}

		if( m_SendUpwards )
		{
			gameObject.SendMessageUpwards( m_Message, options );
		}

		if( !m_Broadcast && !m_SendUpwards )
		{
			gameObject.SendMessage( m_Message, options );
		}
	}
}
