using Events;
using UnityEngine;

namespace Theme
{
    public class BackgroundThemeSelectionElement : ThemeSelectionElementBase
    {
        [SerializeField] private BackgroundsEnum m_backgroundEnum;
        
        public void AddOnclick(BackgroundsEnum backgroundsEnum)
        {
            m_backgroundEnum = backgroundsEnum;
            m_button.onClick.AddListener(() =>
            {
                EventManager.Broadcast(new OnChangeBackgroundEvent(){BackgroundsEnum = m_backgroundEnum});
            });
        }
    }
}