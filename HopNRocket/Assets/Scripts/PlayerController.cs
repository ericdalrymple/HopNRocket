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
	}

	//-- Messages sent by this MonoBehaviour
	private static readonly string MESSAGE_PLAYER_JUMP = "OnPlayerJump";
	private static readonly string MESSAGE_PLAYER_SHOT = "OnPlayerShot";
	private static readonly string MESSAGE_PLAYER_DEAD = "OnPlayerDead";

	//-- Constants
	private static readonly int LAYER_ENEMY = LayerMask.NameToLayer( "Enemy" );
	private static readonly int LAYER_GROUND = LayerMask.NameToLayer( "Ground" );
	private static readonly int LAYER_OBSTACLE = LayerMask.NameToLayer( "Obstacle" );
	private static readonly int LAYER_ENEMY_PROJECTILE = LayerMask.NameToLayer( "EProjectile" );

	//-- Member variables
	public float m_JumpForce;
	public GameObject m_GameWorld;

	private bool m_DisableInput;
	private float m_NativeGravityScale;
	private LinkedList<PlayerAction> m_QueuedActions;
	private Rigidbody2D m_Body;
	private Vector2 m_JumpForceVector;
	private Vector2 m_ResolvedForceVector;
	private Vector2 m_ShotForceVector;

	void Start()
	{
		m_DisableInput = false;
		m_NativeGravityScale = 1.0f;
		m_QueuedActions = new LinkedList<PlayerAction>();
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
	}

	void FixedUpdate()
	{
		ConsumePhysicsActions();
	}

	void NotifyMessage( string message )
	{
		if( null != GameController.Instance )
		{
			GameController.Instance.SendMessage( message, SendMessageOptions.DontRequireReceiver );
		}

		if( null != m_GameWorld )
		{
			m_GameWorld.BroadcastMessage( message, SendMessageOptions.DontRequireReceiver );
		}
	}

	void OnCollisionEnter2D( Collision2D collision )
	{
		//-- Kill the player if it hits the ground, an obstacle, or an enemy projectile
		GameObject colliderObject = collision.gameObject;
		if( (LAYER_ENEMY == colliderObject.layer)
		 || (LAYER_OBSTACLE == colliderObject.layer)
		 || (LAYER_GROUND == colliderObject.layer) )
		{
			KillPlayer();
		}
	}

	void OnTriggerEnter2D( Collider2D collider )
	{
		//-- Kill the player if it hits an enemy projectile
		GameObject colliderObject = collider.gameObject;
		if( (LAYER_ENEMY == colliderObject.layer)
		 || (LAYER_ENEMY_PROJECTILE == colliderObject.layer))
		{
			KillPlayer();
		}
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

	void ConsumePhysicsActions()
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

		//-- Go through the queued actions and act upon the ones
		//   that need to affect physics.
		LinkedListNode<PlayerAction> action = m_QueuedActions.First;
		LinkedListNode<PlayerAction> nextAction = null;
		while( null != action )
		{
			bool consume = true;

			nextAction = action.Next;

			switch( action.Value )
			{
				case PlayerAction.JUMP:
				{
					//-- Downwards jump shot
				    Jump( m_JumpForceVector );
					
					//-- Report the shot or jump to the game controller
				    NotifyMessage( MESSAGE_PLAYER_JUMP );

					break;
				}

				case PlayerAction.SHOOT:
				{
					//-- Jump resulting from an offensive shot
				    Jump( m_ShotForceVector );

					//-- Report the shot or jump to the game controller
					NotifyMessage( MESSAGE_PLAYER_SHOT );

					break;
				}

				default:
				{
					consume = false;
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

	void KillPlayer()
	{
		//-- Report the player's death to the game controller
		NotifyMessage( MESSAGE_PLAYER_DEAD );
	}

	void PollInput()
	{
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
					break;
				}
					
				case PlayerAction.SHOOT:
				{
					actionTriggered = (Input.GetKeyDown( "right" ) || Input.GetButtonDown ( "Fire1" ));
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
