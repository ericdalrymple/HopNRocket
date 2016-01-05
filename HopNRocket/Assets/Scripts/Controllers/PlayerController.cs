using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController
: MonoBehaviour
{
	private enum PlayerAction
	{
		  JUMP = 0
		, SHOOT
		, SPECIAL
	}

	//-- Animator parameter and state hashes
	private static readonly int HASH_TRIGGER_JUMP = Animator.StringToHash( "Jump" );
	private static readonly int HASH_TRIGGER_SHOOT = Animator.StringToHash( "Shoot" );
	private static readonly int HASH_TRIGGER_SPECIAL = Animator.StringToHash( "Special" );
	private static readonly int HASH_INTEGER_SPECIAL_COUNT = Animator.StringToHash( "SpecialCount" );
	private static readonly int HASH_STATE_JUMP = Animator.StringToHash( "Base Layer.JumpCycle.Jump" );
	private static readonly int HASH_STATE_SHOOT = Animator.StringToHash( "Base Layer.ShootCycle.Shoot" );
	private static readonly int HASH_STATE_SPECIAL = Animator.StringToHash( "Base Layer.Special" );

	//-- Constants
	private static readonly string LAUNCHER_ID_JUMP = "Jump";
	private static readonly string LAUNCHER_ID_OFFENSIVE = "Offensive";

	//-- Member variables
	public float m_JumpForce = 5.0f;
	public GameObject m_GameWorld;

	private bool m_DisableInput;
	private float m_NativeGravityScale;
	private Animator m_Animator;
	private LinkedList<PlayerAction> m_QueuedActions = new LinkedList<PlayerAction>();
	private Rigidbody2D m_Body;
	private Vector2 m_JumpForceVector;
	private Vector2 m_ResolvedForceVector;
	private Vector2 m_ShotForceVector;

	void Awake()
	{
		m_DisableInput = false;
		m_NativeGravityScale = 1.0f;
		m_Animator = GetComponent<Animator>();
		m_Body = GetComponent<Rigidbody2D>();
		m_JumpForceVector = new Vector2( 0.0f, m_JumpForce );
		m_ResolvedForceVector = new Vector2();
		m_ShotForceVector = new Vector2( 0.0f, m_JumpForce * Mathf.Cos( 90.0f * Mathf.Deg2Rad ) );

		//-- When the game starts, gravity should not affect the
		//   player object, but we want to use the gravity scale
		//   specified by the developer when gameplay starts.
		if( null != m_Body )
		{
			m_NativeGravityScale = m_Body.gravityScale;
			m_Body.gravityScale = 0.0f;
		}
	}

	void Update()
	{
		PollInput();
		ConsumeActions();
	}

	void OnGameStateChange( GameController.GameStateEvent eventInfo )
	{
		switch( eventInfo.currentState )
		{
			case GameController.GameState.PLAYING:
			{
				m_Body.gravityScale = m_NativeGravityScale;
				break;
			}

			case GameController.GameState.GAME_OVER:
			{
				m_DisableInput = true;
				m_Body.gravityScale = 0.0f;
				m_Body.velocity = Vector2.zero;
				break;
			}
		}
	}

	void ConsumeActions()
	{
		if( null == m_Body )
		{
			//-- No body, no physics
			return;
		}

		if( 0 >= m_QueuedActions.Count )
		{
			//-- No actions queued, early out
			return;
		}

		AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo( 0 );

		//-- Go through the queued actions and act upon the ones
		//   that need to affect physics.
		LinkedListNode<PlayerAction> action = m_QueuedActions.First;
		LinkedListNode<PlayerAction> nextAction = null;
		while( null != action )
		{
			bool consume = false;

			nextAction = action.Next;

			switch( action.Value )
			{
				case PlayerAction.JUMP:
				{
					if( stateInfo.fullPathHash == HASH_STATE_JUMP )
					{
						//-- Jump up
					    Jump( m_JumpForceVector );

						//-- Fire a projectile downwards
						gameObject.BroadcastMessage( AbstractProjectileLauncher.MESSAGE_LAUNCH_PROJECTILE
					                               , LAUNCHER_ID_JUMP );

						consume = true;
					}

					break;
				}

				case PlayerAction.SHOOT:
				{
					if( stateInfo.fullPathHash == HASH_STATE_SHOOT )
					{
						//-- Jump resulting from an offensive shot
					    Jump( m_ShotForceVector );
						
						//-- Fire a projectile forwards
						gameObject.BroadcastMessage( AbstractProjectileLauncher.MESSAGE_LAUNCH_PROJECTILE
						                           , LAUNCHER_ID_OFFENSIVE );

						consume = true;
					}

					break;
				}

				case PlayerAction.SPECIAL:
				{
					if( stateInfo.fullPathHash == HASH_STATE_SPECIAL )
					{
						bool consumed = InventoryManager.instance.ConsumeItem();
						if( consumed )
						{
							//TODO Destroy all turret
							
							//-- Reset the game's scroll speed
							if( WorldScroller.initialized )
							{
								WorldScroller.instance.ResetScrollSpeed();
							}
						}
						
						consume = true;
					}
					
					break;
				}
			}

			if( consume )
			{
				//-- Remove the action
				m_QueuedActions.Remove( action );
			}

			action = nextAction;
		}
	}

	void Jump( Vector2 forceVector )
	{
		//-- Counter the player's existing velocity so that every jump has the same impact on height
		m_ResolvedForceVector.x = forceVector.x;
		m_ResolvedForceVector.y = forceVector.y - m_Body.velocity.y;

		//-- Make the player jump
		m_Body.AddForce( m_ResolvedForceVector, ForceMode2D.Impulse );
	}

	void PollInput()
	{
		//-- Don't poll when input is disable because of game state
		if( m_DisableInput )
		{
			return;
		}

		Array playerActions = Enum.GetValues( typeof( PlayerAction ) );
		foreach( PlayerAction playerAction in playerActions )
		{
			bool actionTriggered = false;

			//-- Query the input assigned to each action
			switch( playerAction )
			{
				case PlayerAction.JUMP:
				{
					actionTriggered = (Input.GetKeyDown( "down" ) || Input.GetButtonDown( "Jump" ));
					if( actionTriggered )
					{
						//-- Flip the jump toggle on the animator
						m_Animator.SetTrigger( HASH_TRIGGER_JUMP );
					}

					break;
				}
					
				case PlayerAction.SHOOT:
				{
					actionTriggered = (Input.GetKeyDown( "right" ) || Input.GetButtonDown ( "Fire1" ));
					if( actionTriggered )
					{
						//-- Flip the shoot toggle on the animator
						m_Animator.SetTrigger( HASH_TRIGGER_SHOOT );
					}

					break;
				}

				case PlayerAction.SPECIAL:
				{
					actionTriggered = (Input.GetKeyDown( "left" ) || Input.GetButtonDown ( "Fire2" ));
					if( actionTriggered )
					{
						//-- Flip the shoot toggle on the animator
						m_Animator.SetInteger( HASH_INTEGER_SPECIAL_COUNT, InventoryManager.instance.itemCount );
						m_Animator.SetTrigger( HASH_TRIGGER_SPECIAL );
					}
					break;
				}
			}

			//-- Queue the action so that is may be affected
			if( actionTriggered )
			{
				m_QueuedActions.AddLast( playerAction );
			}
		}
	}
}
