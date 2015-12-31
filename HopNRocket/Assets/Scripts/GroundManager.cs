using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public class GroundManager
{
    //-- Singleton construct for this manager
    private static GroundManager s_Instance = null;

    public static GroundManager Instance
    {
        get
        {
            if( null == s_Instance )
            {
                s_Instance = new GroundManager();
            }

            return s_Instance;
        }
    }

    private LinkedList<GameObject> m_GroundTiles = new LinkedList<GameObject>();

    public void GenerateGround( float startX, float endX, float groundY, GameObject[] groundTilePrefabs )
    {
        //-- Validate the span
        if( startX > endX )
        {
            //-- Ensure that startX is smaller than endX
            float temp = startX;
            startX = endX;
            endX = startX;
        }
        else if( startX == endX )
        {
            //-- No tiles to lay down since there's no space
            return;
        }

        //-- Validate the number of prefabs
        int tilePrefabCount = groundTilePrefabs.Length;
        if( 0 >= tilePrefabCount )
        {
            //-- Cannot generate tiles without any prefabs
            return;
        }
        
        //-- Initialize useful variables
        float x = startX;
        float tileWidth = 0.0f;
        int tilePrefabIndex = -1;
        BoxCollider2D tileCollider = null;
        GameObject tileInstance = null;
        GameObject tilePrefab = null;
        Vector3 tilePosition = new Vector3( x, groundY, 0.0f );

        //-- Lay down the ground tiles
		bool endReached = false;
		bool loopTilePlaced = false;
        int nullLoopCount = 0;
        int nullLoopCap = 100;
        while( !endReached )
        {
			if( (x > endX) && !loopTilePlaced )
			{
				//-- This loop places the last tile needed to
				//   cover the specified span. This flag will allow
				//   the loop to run an additional time to place a
				//   tile needed for looping the ground seemlessly.
				loopTilePlaced = true;
			}
			else if( loopTilePlaced )
			{
				//-- This loop places the loop tile; it's the last loop.
				endReached = true;
			}

            //-- Randomly select a tile prefab
            tilePrefabIndex = Random.Range( 0, tilePrefabCount );
            tilePrefab = groundTilePrefabs[tilePrefabIndex];

            if( null != tilePrefab )
            {
				tileCollider = tilePrefab.GetComponent<BoxCollider2D>();
				if( null != tileCollider )
				{
					//-- Resolve the tile's width
					tileWidth = tileCollider.size.x;

	                //-- Instantiate the tile
	                tilePosition.x = x;
	                tileInstance = GameObject.Instantiate( tilePrefab
	                                                     , tilePosition
	                                                     , Quaternion.identity ) as GameObject;

                    //-- Add the tile to our list
                    m_GroundTiles.AddLast( tileInstance );

                    

                    //-- Move the marker to the right of the new tile
                    x += tileWidth;

                    //-- We've hit a valid tile, so reset the dud counter
                    nullLoopCount = 0;
                }
                else
                {
                    //-- We've hit a dud

                    //-- Dud width is 0
                    tileWidth = 0.0f;

                    //-- Break if we've hit too many consecutive duds; probably means
                    //   that none of the prefabs are valid tiles
                    if( ++nullLoopCount >= nullLoopCap )
                    {
                        break;
                    }
                }

                Assert.IsNotNull<BoxCollider2D>( tileCollider
                                               , "Tile prefab at index " + tilePrefabIndex + " has no BoxCollider2D." );
            }

            Assert.IsNotNull<GameObject>( tilePrefab
                                        , "Tile prefab at index " + tilePrefabIndex + " is null." );
        }
    }

    public bool ReEnqueue( GameObject groundTile )
    {
        if( m_GroundTiles.Count <= 0 )
        {
            //-- Avoids edge cases later on
            return false;
        }

        //-- Find the specified tile in the list (in principle, should always be the first tile, but using 'Find' just in case)
        LinkedListNode<GameObject> node = m_GroundTiles.Find( groundTile );
        if( null == node )
        {
            //-- Ground tile is not managed by this GroundManager
            return false;
        }

        //-- Get the colliders for the tile and for the last tile in the list so we can
        //   precisely reposition the tile we are re-enqueueing
        GameObject lastTile = m_GroundTiles.Last.Value;
        BoxCollider2D lastTileCollider = lastTile.GetComponent<BoxCollider2D>();
        BoxCollider2D currentTileCollider = groundTile.GetComponent<BoxCollider2D>();
        if( (null == lastTileCollider) || (null == currentTileCollider) )
        {
            //-- Cannot perform operation without missing position/dimension information
            return false;
        }

        //-- Reposition the tile we want to re-enqueue
        float lastTileRightX = lastTile.transform.position.x + lastTileCollider.size.x;
        groundTile.transform.Translate( lastTileRightX - groundTile.transform.position.x
                                      , 0.0f
                                      , 0.0f );

        //-- Re-enqueue the tile
        m_GroundTiles.Remove( node );
        m_GroundTiles.AddLast( node );

        return true;
    }
}
