using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private PlayerScoreUI m_playerScoreUI;

        [SerializeField] private PlayerTurnIndicatorUI m_playerTurnIndicatorUI;

        public void SetScore(int score)
        {
            m_playerScoreUI.SetScore(score);
            m_playerScoreUI.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), .75f);
        }

        public void ShowIndicator()
        {
            m_playerTurnIndicatorUI.ChangeColor(true);
        }

        public void HideIndicator()
        {
            m_playerTurnIndicatorUI.ChangeColor(false);
        }
        
    }
}