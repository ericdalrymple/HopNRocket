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

	void OnProximityEnter( GameObject target )
	{
		if( target.CompareTag( "Player" ) )
		{
			if( null != m_Animator )
			{
				m_Animator.SetBool( m_HashPlayerDetected, true );
			}
		}
	}

	void OnProximityExit( GameObject target )
	{
		if( target.CompareTag( "Player" ) )
		{
			if( null != m_Animator )
			{
				m_Animator.SetBool( m_HashPlayerDetected, false );
			}
		}
	}
}
