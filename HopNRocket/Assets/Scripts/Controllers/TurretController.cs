using UnityEngine;
using System.Collections;

public class TurretController
: MonoBehaviour
{
	public int m_MaxHP;
	public int m_Score;

	//-- Animator parameter hashes
	private static readonly int m_HashPlayerDetected = Animator.StringToHash( "PlayerDetected" );
	private static readonly int m_HashRemainingHits  = Animator.StringToHash( "RemainingHits" );

	private int m_HP;
	private Animator m_Animator;

	void Start()
	{
		m_Animator = GetComponent<Animator>();

		//-- Initialize HP
		SetHP( m_MaxHP );
	}

	void Update()
	{
	
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

	void OnHit()
	{
		SetHP( m_HP - 1 );
	}

	void SetHP( int newHP )
	{
		//-- Update HP to new value
		m_HP = Mathf.Max( 0, newHP );

		//-- Synch the value to the animator
		if( null != m_Animator )
		{
			m_Animator.SetInteger( m_HashRemainingHits, m_HP );
		}
	}
}
