using TMPro;
using UnityEngine;

namespace UI
{
    public class RoundUI : UIElementBase
    {
        [SerializeField] private TextMeshProUGUI m_roundText;
        public void SetText(int round)
        {
            m_roundText.SetText(round.ToString());
        }
    }
}