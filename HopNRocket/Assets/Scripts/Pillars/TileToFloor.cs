using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class TileToFloor
: MonoBehaviour
{
	private SpriteRenderer m_Renderer;

	void Start()
	{
		m_Renderer = GetComponent<SpriteRenderer>();

		GameObject gameAreaObject = GameObject.FindGameObjectWithTag( "GameArea" );
		if( null != gameAreaObject )
		{
			BoxCollider2D gameArea = gameAreaObject.GetComponent<BoxCollider2D>();
			if( null != gameArea )
			{
				GenerateTiles( gameArea.bounds.min.y );
			}
		}
	}

	void GenerateTiles( float floorY )
	{
		Bounds spriteBounds = m_Renderer.sprite.bounds;

		Vector3 realMax = transform.TransformPoint( spriteBounds.max );
		Vector3 realMin = transform.TransformPoint( spriteBounds.min );
		Vector3 realSize = realMax - realMin;

		//-- Compute how many tiles we need to get to the floor
		float toFloor = transform.position.y - floorY;
		int tileCount = (int)(toFloor / realSize.y);
		if( 0 >= tileCount )
		{
			//-- No tiles to lay
			return;
		}

		//-- Create a prefab consisting of the sprite we want to tile
		GameObject tilePrefab = new GameObject();
		SpriteRenderer tilePrefabRenderer = tilePrefab.AddComponent<SpriteRenderer>();
		tilePrefabRenderer.transform.position = transform.position;
		tilePrefabRenderer.sprite = m_Renderer.sprite;

		//-- Make a tile position that we will be moving down
		Vector3 tileSpacing = new Vector3( 0.0f
		                                 , realSize.y
		                                 , 0.0f );

		Vector3 localScale = new Vector3( 1.0f, 1.0f, 1.0f );

		GameObject tileBuffer;
		for( int i = 0; i < tileCount; ++i )
		{
			tileBuffer = Instantiate( tilePrefab ) as GameObject;
			tileBuffer.transform.SetParent( transform.parent );
			tileBuffer.transform.position = transform.position - tileSpacing * (i + 1);
			tileBuffer.transform.localScale = localScale;
		}

		//-- Don't need the prefab anymore
		Destroy( tilePrefab );
	}
}
