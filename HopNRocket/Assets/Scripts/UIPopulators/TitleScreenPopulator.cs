using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class TitleScreenPopulator
: MonoBehaviour
{
	public Text m_TurretPointsText;
	public Text m_TankPointsText;

	public Killable m_TurretKillable;
	public Killable m_TankKillable;

	private StringBuilder m_ConcatBuffer = new StringBuilder();

	void Start()
	{
		if( (null != m_TurretPointsText) && (null != m_TurretKillable) )
		{
			m_ConcatBuffer.Length = 0;
			m_ConcatBuffer.Append( m_TurretKillable.scoreValue );
			m_TurretPointsText.text = m_ConcatBuffer.ToString();
		}

		if( (null != m_TankPointsText) && (null != m_TankKillable) )
		{
			m_ConcatBuffer.Length = 0;
			m_ConcatBuffer.Append( m_TankKillable.scoreValue );
			m_TankPointsText.text = m_ConcatBuffer.ToString();
		}
	}
}
