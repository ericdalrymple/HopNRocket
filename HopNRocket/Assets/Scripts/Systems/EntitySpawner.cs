using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class EntitySpawner
: GameControllerSystem<EntitySpawner>
{
	/**
	 * Custom padding struct to improve editor interface.
	 */
	[System.Serializable]
	public class SpawnerPadding
	{
		public float top;
		public float bottom;
	}

	//-- Constants
	private static readonly float OFFSCREEN_SPAWN_PADDING = 2.0f;


	//-- Settings
	public float m_InitialSpawnDelay;
	public float m_CollectibleSpawnChance;
	public int m_TurretSpawnInterval;
	public SpawnerPadding m_Padding;

	public GameObject[] m_CollectiblePrefabs;
	public GameObject[] m_TurretPrefabs;


	//-- Members
	private bool m_FirstTick = true;
	private Vector2 m_SpawnPositionRange = new Vector2();

	void Start()
	{

	}

	void Update()
	{
		if( m_FirstTick )
		{
			//-- Initialize the spawn range on the first tick. This
			//   helps ensure that the ground has been spawned before
			//   deciding a spawn range.
			InitializeSpawnRange();

			//-- Start spawn loops
			StartCoroutine( SpawnLoopCollectibles() );
			StartCoroutine( SpawnLoopTurrets() );

			//-- Mark first tick complete
			m_FirstTick = false;
		}
	}

	float GenerateSpawnHeight()
	{
//		Random.Range( m_GameArea.b );
		return 0.0f;
	}

	void InitializeSpawnRange()
	{
		BoxCollider2D gameArea = null;
		GameObject gameAreaObject = GameObject.FindGameObjectWithTag( "GameArea" );
		if( null != gameAreaObject )
		{
			gameArea = gameAreaObject.GetComponent<BoxCollider2D>();
		}
		
		Assert.IsNotNull( gameArea, "The game has no GameArea or its GameArea has not been initialized." );
		if( null != gameArea )
		{
			m_SpawnPositionRange.x = GroundManager.instance.surfaceY;
			m_SpawnPositionRange.y = gameArea.bounds.max.y;
		}
	}

	IEnumerator SpawnLoopCollectibles()
	{
		//-- Wait for the initial spawn duration plus half of another
		//   one so that our collectibles spawn between turrets
		yield return new WaitForSeconds( m_InitialSpawnDelay + m_TurretSpawnInterval * 0.5f );
		
		while( GameController.instance.IsGamePlaying() )
		{
			//-- Roll the dice to see if we should spawn a collectible
			if( Random.value > m_CollectibleSpawnChance )
			{
				SpawnCollectible();
			}

			//-- Wait another spawn interval
			yield return new WaitForSeconds( m_TurretSpawnInterval );
		}
	}

	IEnumerator SpawnLoopTurrets()
	{
		yield return new WaitForSeconds( m_InitialSpawnDelay );

		while( GameController.instance.IsGamePlaying() )
		{
			//-- Spawn a turret
			SpawnTurret();

			//-- Wait another spawn interval
			yield return new WaitForSeconds( m_TurretSpawnInterval );
		}
	}

	void SpawnCollectible()
	{
		
	}

	void SpawnTurret()
	{

	}
}
