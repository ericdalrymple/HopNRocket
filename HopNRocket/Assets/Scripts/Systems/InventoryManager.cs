using UnityEngine;
using System.Collections;

public class InventoryManager
: GameControllerSystem<InventoryManager>
{
	//-- Attributes
	public int itemCount{ get{ return m_ItemCount; } }

	public int m_StartingItemCount = 1;

	private int m_ItemCount;

	void Start()
	{
		m_ItemCount = m_StartingItemCount;
	}

	void OnLevelWasLoaded()
	{
		m_ItemCount = m_StartingItemCount;
	}

	/**
	 * Increases the number of available items by a
	 * specified amount.
	 */
	public void AddItems( uint quantity )
	{
		m_ItemCount += (int)quantity;
	}

	/**
	 * Decreases the amount of available items by one. If
	 * there are none left already, this function will return
	 * 'false' and will not decrement into the negatives, otherwise
	 * it will return 'true'.
	 */
	public bool ConsumeItem()
	{
		if( 0 < m_ItemCount )
		{
			--m_ItemCount;
			return true;
		}

		return false;
	}
}
