using Events;
using UnityEngine;


namespace Theme
{
    public class CardFrontThemeSelectionElement : ThemeSelectionElementBase
    {
        [SerializeField] private CardFrontsEnum m_cardFrontsEnum;

        public void AddOnclick(CardFrontsEnum cardFrontsEnum)
        {
            m_cardFrontsEnum = cardFrontsEnum;
            m_button.onClick.AddListener(() =>
            {
                EventManager.Broadcast(new OnChangeCardFrontEvent(){CardFrontEnum = m_cardFrontsEnum});
            });
        }
    }
}
