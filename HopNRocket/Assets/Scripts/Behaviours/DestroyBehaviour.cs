 using UnityEngine;
using System.Collections;

[SharedBetweenAnimators]
public class DestroyBehaviour
: StateMachineBehaviour
{
	public bool m_DestroyParent = false;

	public GameObject m_Explosion = null;
	public bool m_ScaleExplosion = true;

	override public void OnStateEnter( Animator animator, AnimatorStateInfo stateInfo, int layerIndex )
	{
		Transform animatorTransform = animator.transform;
		Transform targetTransform = m_DestroyParent? animatorTransform.parent : animatorTransform;

		if( null != m_Explosion )
		{
			GameObject explosion = Instantiate( m_Explosion
			                                  , targetTransform.position
			           						  , Quaternion.identity ) as GameObject;

			//-- Transform the explosion to its subject just in case it looks directional
			explosion.transform.rotation = Quaternion.RotateTowards( explosion.transform.rotation
			                                                       , targetTransform.rotation
			                                                       , 360.0f );

			if( m_ScaleExplosion )
			{
				explosion.transform.localScale = Vector3.Scale( explosion.transform.localScale
				                                              , targetTransform.localScale );
			}
		}

		Destroy( targetTransform.gameObject );
	}
}
