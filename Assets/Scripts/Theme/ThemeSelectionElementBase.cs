using UnityEngine;
using UnityEngine.UI;

namespace Theme
{
    public abstract class ThemeSelectionElementBase : MonoBehaviour
    {
        [SerializeField] private Image m_preview;
        [SerializeField] protected Button m_button;

        public void SetSprite(Sprite sprite)
        {
            m_preview.sprite = sprite;
        }
    }
}