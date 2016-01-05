using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class AffectScrollSpeedBehaviour
: StateMachineBehaviour
{
	public enum ScrollSpeedDirection
	{
		  INCREMENT = 0
		, DECREMENT
	}

	[Tooltip( "Whether this behaviour increments or decrements the game's scroll speed" )]
	public ScrollSpeedDirection m_Mode;

	[Tooltip( "Number of times to increment or decrement the game scroll speed" )]
	public int m_Count = 1;

	public override void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		switch( m_Mode )
		{
		case ScrollSpeedDirection.INCREMENT:
		{
			for( int i = 0; i < m_Count; ++i )
			{
				WorldScroller.instance.IncrementScrollSpeed();
			}

			break;
		}

		case ScrollSpeedDirection.DECREMENT:
		{
			for( int i = 0; i < m_Count; ++i )
			{
				WorldScroller.instance.DecrementScrollSpeed();
			}

			break;
		}
		}
	}
}
