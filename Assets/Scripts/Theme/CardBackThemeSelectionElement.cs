using Events;
using UnityEngine;

namespace Theme
{
    public class CardBackThemeSelectionElement : ThemeSelectionElementBase
    { 
        [SerializeField] private CardBackEnum m_cardBackEnum;

        public void AddOnclick(CardBackEnum cardBackEnum)
        {
            m_cardBackEnum = cardBackEnum;
            m_button.onClick.AddListener(() =>
            {
                EventManager.Broadcast(new OnChangeCardBackEvent() { CardBackEnum = m_cardBackEnum });
            });
        }
    }
}