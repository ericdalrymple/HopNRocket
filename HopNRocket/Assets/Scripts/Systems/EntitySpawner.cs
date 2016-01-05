using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class EntitySpawner
: MonoBehaviour
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
	private static readonly float OFFSCREEN_SPAWN_PADDING = 3.0f;
	private static readonly Vector3 FLIP_SCALE_Y = new Vector3( 1.0f, -1.0f, 1.0f );


	//-- Settings
	public float m_InitialSpawnDelay;
	public float m_CollectibleSpawnChance;
	public float m_TurretSpawnInterval;
	public SpawnerPadding m_Padding;

	public GameObject m_FloorPillarPrefab;
	public GameObject m_CeilingPillarPrefab;
	public GameObject[] m_CollectiblePrefabs;
	public GameObject[] m_TurretPrefabs;


	//-- Members
	private bool m_FirstTick = true;
	private float m_SpawnPositionX;
	private float m_SpawnAreaCenterY;
	private Vector2 m_SpawnPositionRange = new Vector2();

	void Update()
	{
		if( m_FirstTick )
		{
			//-- Initialize the spawn range on the first tick. This
			//   helps ensure that the ground has been spawned before
			//   deciding a spawn range.
			InitializeSpawnRange();

			//-- Mark first tick complete
			m_FirstTick = false;
		}
	}

	void InitializeSpawnRange()
	{
		//-- Access the game area object
		BoxCollider2D gameArea = null;
		GameObject gameAreaObject = GameObject.FindGameObjectWithTag( "GameArea" );
		if( null != gameAreaObject )
		{
			gameArea = gameAreaObject.GetComponent<BoxCollider2D>();
		}

		Assert.IsNotNull( gameArea, "The game has no GameArea or its GameArea has not been initialized." );
		if( null != gameArea )
		{
			//-- Compute the vertical spawning range
			m_SpawnPositionRange.x = GroundManager.instance.surfaceY + m_Padding.bottom;
			m_SpawnPositionRange.y = gameArea.bounds.max.y - m_Padding.top;

			//-- Compute the center of the spawning area
			m_SpawnAreaCenterY = (m_SpawnPositionRange.x + m_SpawnPositionRange.y) * 0.5f;

			//-- Compute the x-position at which to spawn objects
			m_SpawnPositionX = gameArea.bounds.max.x + OFFSCREEN_SPAWN_PADDING;
		}
	}

	void OnGameStateChange( GameController.GameStateEvent eventInfo )
	{
		if( GameController.GameState.PLAYING == eventInfo.currentState )
		{
			//-- Start spawn loops when game starts
			StartCoroutine( SpawnLoopCollectibles() );
			StartCoroutine( SpawnLoopTurrets() );
		}
	}

	float GenerateSpawnY()
	{
		return Random.Range( m_SpawnPositionRange.x, m_SpawnPositionRange.y );
	}

	IEnumerator SpawnLoopCollectibles()
	{
		//-- Wait for the initial spawn duration plus half of another
		//   one so that our collectibles spawn between turrets
		yield return new WaitForSeconds( m_InitialSpawnDelay + m_TurretSpawnInterval * 0.5f );
		
		while( GameController.instance.IsGamePlaying() )
		{
			//-- Roll the dice to see if we should spawn a collectible
			if( Random.value <= m_CollectibleSpawnChance )
			{
//				SpawnCollectible();
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
			//-- Roll the dice to see if we should spawn a collectible
			if( Random.value <= m_CollectibleSpawnChance )
			{
				SpawnCollectible();
			}
			else
			{
				//-- Spawn a turret
				SpawnTurret();
			}

			//-- Wait another spawn interval
			yield return new WaitForSeconds( m_TurretSpawnInterval / WorldScroller.instance.relativeScrollSpeed );
		}
	}

	void SpawnCollectible()
	{
		//-- Pick a collectible at random
		int collectibleIndex = Random.Range( 0, m_CollectiblePrefabs.Length );
		
		//-- Pick a position along the y-axis at the horizontal spawn location
		Vector3 spawnPosition = new Vector3( m_SpawnPositionX
		                                   , GenerateSpawnY()
		                                   , 0.0f );
		
		//-- Spawn the collectible
		Instantiate( m_CollectiblePrefabs[collectibleIndex]
                   , spawnPosition
                   , Quaternion.identity );
	}

	void SpawnTurret()
	{
		//-- Pick a turret at random
		int turretIndex = Random.Range( 0, m_TurretPrefabs.Length );

		//-- Pick a position along the y-axis at the horizontal spawn location
		Vector3 spawnPosition = new Vector3( m_SpawnPositionX
										   , GenerateSpawnY()
										   , 0.0f );

		//-- Spawn the turret
		GameObject turretInstance = Instantiate( m_TurretPrefabs[turretIndex]
									           , spawnPosition
									           , Quaternion.identity ) as GameObject;

		//-- We need to flip it if it's in the top half of the spawn area
		if( spawnPosition.y > m_SpawnAreaCenterY )
		{
			Vector3 flippedScale = Vector3.Scale( turretInstance.transform.localScale, FLIP_SCALE_Y );
			turretInstance.transform.localScale = flippedScale;

			//-- Spawn ceiling pillar
			if( null != m_CeilingPillarPrefab )
			{
				//-- Spawn floor pillar
				GameObject pillarInstance = Instantiate( m_CeilingPillarPrefab
				                                       , spawnPosition
				                                       , Quaternion.identity ) as GameObject;
			}
		}
		else
		{
			if( null != m_FloorPillarPrefab )
			{
				//-- Spawn floor pillar
				GameObject pillarInstance = Instantiate( m_FloorPillarPrefab
				                                       , spawnPosition
				                                       , Quaternion.identity ) as GameObject;
			}
		}

		//-- Set the Enemies game object as the new turret's parent
		turretInstance.transform.SetParent( EnemyCollection.instance.gameObject.transform );
	}
}
