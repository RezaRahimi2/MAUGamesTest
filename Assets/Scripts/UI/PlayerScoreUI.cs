using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerScoreUI : UIElementBase
    {
        [SerializeField] private TextMeshProUGUI m_scoreText;

        public void SetScore(int score)
        {
            m_scoreText.SetText(score.ToString());
        }
    }
}