using UnityEngine;
using System.Collections;

public class LayerSelector
: MonoBehaviour
{
	public string[] m_TargetLayerNames;
	
	private int[] m_TargetLayerIndices;
	
	void Awake()
	{
		//-- Cache the indices for all of our target layers
		int targetLayerCount = m_TargetLayerNames.Length;
		
		m_TargetLayerIndices = new int[targetLayerCount];
		for( int i = 0; i < targetLayerCount; ++i )
		{
			m_TargetLayerIndices[i] = LayerMask.NameToLayer( m_TargetLayerNames[i] );
		}
	}

	protected bool IsOnTargetLayer( GameObject gameObject )
	{
		foreach( int layer in m_TargetLayerIndices )
		{
			if( layer == gameObject.layer )
			{
				return true;
			}
		}
		
		return false;
	}
}
