using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerTurnIndicatorUI : UIElementBase
    {
        [SerializeField] private Image m_indicator;
        [SerializeField] private Tweener m_tweener;
        public void ChangeColor(bool isTurn)
        {
            if(isTurn)
                m_tweener = AnimationManager.Instance.ChangeColor(m_indicator,Color.green);
            else
            {
                AnimationManager.Instance.KillTween(m_tweener);
                AnimationManager.Instance.ChangeColor(m_indicator, Color.red);
            }
        }
    }
}