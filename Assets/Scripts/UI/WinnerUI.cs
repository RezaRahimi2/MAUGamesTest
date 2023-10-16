using TMPro;
using UnityEngine;

namespace UI
{
    public class WinnerUI : UIElementBase
    {
        [SerializeField] private TextMeshProUGUI m_playerIndexText;
        public void SetText(int playerIndex)
        {
            m_playerIndexText.SetText(playerIndex.ToString());
        }
    }
}