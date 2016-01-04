using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class HUDPopulator
: MonoBehaviour
{
	[Tooltip( "Rate of value change in points per second" )]
	public float m_ScoreTrickleRate;

	public Text m_BestScoreText;
	public Text m_ScoreText;
	public Text m_TokenText;

	private float m_CurrentScore = 0;
	private float m_NewScore = 0;
	private float m_OldScore = 0;

	private StringBuilder m_ConcatBuffer = new StringBuilder();

	void Start()
	{
		if( null != m_BestScoreText )
		{
			//-- Immediately set the value to the best score
			m_ConcatBuffer.Length = 0;
			m_ConcatBuffer.Append( ScoreManager.instance.bestScore );
			m_BestScoreText.text = m_ConcatBuffer.ToString();
		}
	}

	void Update()
	{
		UpdateScoreLabel();
		UpdateTokenLabel();
	}

	void UpdateScoreLabel()
	{
		bool refresh = false;

		float backup = m_NewScore;
		m_NewScore = ScoreManager.instance.totalScore;
		if( backup != m_NewScore )
		{
			//-- Update the 'old score' every time the score changes
			m_OldScore = m_CurrentScore;
		}

		if( m_CurrentScore != m_NewScore )
		{
			if( 0 == m_ScoreTrickleRate )
			{
				//-- No trickle, just set the score
				m_CurrentScore = m_NewScore;
			}
			else
			{
				//-- Trickle the score value
				m_CurrentScore += m_ScoreTrickleRate * Time.deltaTime * Mathf.Sign( m_NewScore - m_CurrentScore );
				if( 0.0f > ((m_NewScore - m_CurrentScore) * (m_NewScore - m_OldScore)) )
				{
					//-- We've exceeded the target score; clamp it
					m_CurrentScore = m_NewScore;
				}
			}

			refresh = true;
		}

		//-- Refresh if there is no value in the label
		refresh = refresh || (m_ScoreText.text.Length == 0);

		if( refresh )
		{
			//-- Put the score value in the label
 			m_ConcatBuffer.Length = 0;
			m_ConcatBuffer.Append( (int)m_CurrentScore );
			m_ScoreText.text = m_ConcatBuffer.ToString();
		}
	}

	void UpdateTokenLabel()
	{

	}
}
