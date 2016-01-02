using UnityEngine;
using System.Collections;

public class PlayerController
: MonoBehaviour
{
	//-- Messages sent by this MonoBehaviour
	private static readonly string MESSAGE_PLAYER_JUMP = "OnPlayerJump";
	private static readonly string MESSAGE_PLAYER_SHOT = "OnPlayerShot";
	private static readonly string MESSAGE_PLAYER_DEAD = "OnPlayerDead";

	//-- Constants
	private static readonly int LAYER_GROUND = LayerMask.NameToLayer( "Ground" );
	private static readonly int LAYER_OBSTACLE = LayerMask.NameToLayer( "Obstacle" );
	private static readonly int LAYER_ENEMY_PROJECTILE = LayerMask.NameToLayer( "EProjectile" );

	//-- Member variables
	public float m_JumpForce;
	public GameObject m_GameWorld;

	private bool m_JumpAction;
	private bool m_ShotAction;
	private float m_NativeGravityScale;
	private Rigidbody2D m_Body;
	private Vector2 m_JumpForceVector;
	private Vector2 m_ResolvedForceVector;
	private Vector2 m_ShotForceVector;

	void Start()
	{
		m_JumpAction = false;
		m_ShotAction = false;
		m_NativeGravityScale = 1.0f;
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
		if( Input.GetKeyDown( "down" ) || Input.GetButtonDown( "Jump" ) )
		{
			m_JumpAction = true;
		}

		if( Input.GetKeyDown( "right" ) || Input.GetButtonDown ( "Fire1" ) )
		{
			m_ShotAction = true;
		}
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
		if( (LAYER_GROUND == colliderObject.layer) || (LAYER_OBSTACLE == colliderObject.layer) )
		{
			KillPlayer();
		}
	}

	void OnGameStateChange( GameController.GameStateEvent eventInfo )
	{
		if( GameController.GameState.PLAYING == eventInfo.currentState )
		{
			m_Body.gravityScale = m_NativeGravityScale;
		}
	}

	void ConsumePhysicsActions()
	{
		if( null == m_Body )
		{
			return;
		}

		string shotEvent = null;
		Vector2 jumpVector = Vector2.zero;

		if( m_JumpAction )
		{
			//-- Setup the downwards jump shot
			jumpVector = m_JumpForceVector;
			shotEvent = MESSAGE_PLAYER_JUMP;
			m_JumpAction = false;
		}
		else if( m_ShotAction )
		{
			//-- Setup the forward offensive jump
			jumpVector = m_ShotForceVector;
			shotEvent = MESSAGE_PLAYER_SHOT;
			m_ShotAction = false;
		}
		else
		{
			return;
		}

		//-- Counter the player's existing velocity so that every jump has the same impact on height
		m_ResolvedForceVector.x = jumpVector.x;
		m_ResolvedForceVector.y = jumpVector.y - m_Body.velocity.y;

		//-- Make the player jump
		m_Body.AddForce( m_ResolvedForceVector, ForceMode2D.Impulse );
		
		//-- Report the shot or jump to the game controller
		NotifyMessage( shotEvent );
	}

	void KillPlayer()
	{
		//-- Report the player's death to the game controller
		NotifyMessage( MESSAGE_PLAYER_DEAD );
	}
}
