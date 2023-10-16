using Managers;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Theme
{
    public class ThemeSelectionPanelUI : UIElementBase
    {
        [SerializeField] private Button m_closeButton;
        [SerializeField] private Transform m_cardsFrontContent;
        [SerializeField] private Transform m_cardsBackContent;
        [SerializeField] private Transform m_backgroundContent;

        public void Initialize()
        {
            m_closeButton.onClick.AddListener(Hide);
            
            ThemeCustomizationData themeCustomizationData = DataManager.Instance.ThemeCustomizationData;
            
            themeCustomizationData.CradsFrontData.ForEach(x =>
            {
                CardFrontThemeSelectionElement cardFrontThemeSelectionElement =
                    Instantiate(PrefabsManager.Instance.CardFrontElementPrefab, m_cardsFrontContent);
                
                cardFrontThemeSelectionElement.SetSprite(x.PreviewSprite);
                cardFrontThemeSelectionElement.AddOnclick(x.Type);
                
            });
            
            themeCustomizationData.CardsBackData.ForEach(x =>
            {
                CardBackThemeSelectionElement cardBackThemeSelectionElement =
                    Instantiate(PrefabsManager.Instance.CardBackElementPrefab, m_cardsBackContent);
                
                cardBackThemeSelectionElement.SetSprite(x.PreviewSprite);
                cardBackThemeSelectionElement.AddOnclick(x.Type);
            });
            
            themeCustomizationData.BackgroundsData.ForEach(x =>
            {
                BackgroundThemeSelectionElement backgroundThemeSelection =
                    Instantiate(PrefabsManager.Instance.BackgroundElementPrefab, m_backgroundContent);
                
                backgroundThemeSelection.SetSprite(x.Thumbnail);
                backgroundThemeSelection.AddOnclick(x.Type);
            });
        }
        
        public void Show()
        {
            CanvasGroup.alpha = 0;
            gameObject.SetActive(true);
            AnimationManager.Instance.UIFadeAnimation(CanvasGroup, 1);
        }

        public void Hide()
        {
            AnimationManager.Instance.UIFadeAnimation(CanvasGroup, 0,()=>
               gameObject.SetActive(false) );
        }

    }
}
