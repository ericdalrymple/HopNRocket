 using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class DestroyBehaviour
: StateMachineBehaviour
{
	public GameObject m_Explosion = null;

	override public void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		if( null != m_Explosion )
		{
			GameObject explosion = Instantiate( m_Explosion
			           						  , animator.gameObject.transform.position
			           						  , Quaternion.identity ) as GameObject;

			//-- Transform the explosion to its subject just in case it looks directional
			explosion.transform.localScale = animator.transform.localScale;
			explosion.transform.position = animator.transform.position;
			explosion.transform.rotation = animator.transform.rotation;
		}

		Destroy( animator.gameObject );
	}
}
