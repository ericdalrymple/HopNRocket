using UnityEngine;
using System.Collections;

public class PlayerController
: MonoBehaviour
{
	public float m_JumpForce;

	private bool m_DownShotQueued;
	private bool m_ForwardShotQueued;
	private GameObject m_GameController;
	private Rigidbody2D m_Body;
	private Vector2 m_JumpForceVector;
	private Vector2 m_ResolvedForceVector;
	private Vector2 m_ShotForceVector;

	void Start()
	{
		m_DownShotQueued = false;
		m_ForwardShotQueued = false;
		m_GameController = GameObject.FindGameObjectWithTag( "GameController" );
		m_Body = GetComponent<Rigidbody2D>();
		m_JumpForceVector = new Vector2( 0.0f, m_JumpForce );
		m_ResolvedForceVector = new Vector2();
		m_ShotForceVector = new Vector2( 0.0f, m_JumpForce * Mathf.Cos( 90.0f * Mathf.Deg2Rad ) );
	}

	void Update()
	{
		if( Input.GetKeyDown( "down" ) || Input.GetButtonDown( "Jump" ) )
		{
			m_DownShotQueued = true;
		}

		if( Input.GetKeyDown( "right" ) || Input.GetButtonDown ( "Fire1" ) )
		{
			m_ForwardShotQueued = true;
		}
	}

	void FixedUpdate()
	{
		ConsumeShot();
	}

	void ConsumeShot()
	{
		if( null == m_Body )
		{
			return;
		}

		string shotEvent = null;
		Vector2 jumpVector = Vector2.zero;

		if( m_DownShotQueued )
		{
			//-- Setup the downwards jump shot
			jumpVector = m_JumpForceVector;
			shotEvent = "OnPlayerShootDown";
			m_DownShotQueued = false;
		}
		else if( m_ForwardShotQueued )
		{
			//-- Setup the forward offensive jump
			jumpVector = m_ShotForceVector;
			shotEvent = "OnPlayerShootForward";
			m_ForwardShotQueued = false;
		}
		else
		{
			return;
		}

		//-- Counter the player's downward velocity
		m_ResolvedForceVector.x = jumpVector.x;
		m_ResolvedForceVector.y = jumpVector.y - Mathf.Min( 0.0f, m_Body.velocity.y );

		//-- Make the player jump
		m_Body.AddForce( m_ResolvedForceVector, ForceMode2D.Impulse );
		
		//-- Report the jump to the game controller
		if( null != m_GameController )
		{
			m_GameController.SendMessage( shotEvent );
		}
	}
}
