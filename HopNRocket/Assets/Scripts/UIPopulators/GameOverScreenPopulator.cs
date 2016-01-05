using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class GameOverScreenPopulator
: MonoBehaviour
{
	public Text m_BestScoreText;
	public Text m_ScoreText;

	private StringBuilder m_ConcatBuffer = new StringBuilder();

	void OnEnable()
	{
		if( !ScoreManager.initialized )
		{
			return;
		}

 		if( null != m_BestScoreText )
		{
			m_ConcatBuffer.Length = 0;
			m_ConcatBuffer.Append( ScoreManager.instance.bestScore );
			m_BestScoreText.text = m_ConcatBuffer.ToString();
		}

		if( null != m_ScoreText )
		{
			m_ConcatBuffer.Length = 0;
			m_ConcatBuffer.Append( ScoreManager.instance.totalScore );
			m_ScoreText.text = m_ConcatBuffer.ToString();
		}
	}
}
