using UnityEngine;
using System.Collections;

/**
 * A very simple killable component that takes one hit point of
 * damage on every hit and dies when it runs out of HP.
 */
public class Killable
: MonoBehaviour
{
	//-- Supported messages
	public static readonly string MESSAGE_HIT = "OnHit";
	public static readonly string MESSAGE_KILL = "OnKill";

	//-- Animator Hases
	private static readonly int HASH_INTEGER_HP = Animator.StringToHash( "HP" );
	private static readonly int HASH_TRIGGER_DEAD = Animator.StringToHash( "Dead" );
	private static readonly int HASH_TRIGGER_HIT = Animator.StringToHash( "Hit" );

	//-- Settings
	public bool m_DestroyOnDeath = false;
	public int m_StartingHP = 1;

	//-- Members
	private int m_CurrentHP;
	private Animator m_Animator;

	void Start()
	{
		m_CurrentHP = m_StartingHP;
		m_Animator = GetComponent<Animator>();
	}

	void OnHit()
	{
		Hit();
	}

	void OnKill()
	{
		Die();
	}

	void Hit()
	{
		if( 0 >= m_CurrentHP )
		{
			//-- Do nothing if this object is already out of HP
			return;
		}

		//-- Update HP
		--m_CurrentHP;

		//-- Update animator
		m_Animator.SetInteger( HASH_INTEGER_HP, m_CurrentHP );
		m_Animator.SetTrigger( HASH_TRIGGER_HIT );

		//-- Die if we ran out of HP
		if( 0 >= m_CurrentHP )
		{
			Die();
		}
	}

	void Die()
	{
		//-- Update animator
		m_Animator.SetTrigger( HASH_TRIGGER_DEAD );

		//-- Destroy parent object if needed
		if( m_DestroyOnDeath )
		{
			Destroy( gameObject );
		}
	}
}
