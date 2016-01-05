using UnityEngine;
using System.Collections;

public class TurretController
: MonoBehaviour
{
	//-- Animator parameter hashes
	private static readonly int m_HashPlayerDetected = Animator.StringToHash( "PlayerDetected" );

	private Animator m_Animator;

	void Start()
	{
		m_Animator = GetComponent<Animator>();
	}

	void OnTriggerEnter2D( Collider2D collider )
	{
		if( collider.gameObject.CompareTag( "Player" ) )
		{
			if( null != m_Animator )
			{
				m_Animator.SetBool( m_HashPlayerDetected, true );
			}
		}
	}

	void OnTriggerExit2D( Collider2D collider )
	{
		if( collider.gameObject.CompareTag( "Player" ) )
		{
			if( null != m_Animator )
			{
				m_Animator.SetBool( m_HashPlayerDetected, false );
			}
		}
	}
}
