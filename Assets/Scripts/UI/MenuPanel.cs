using System;
using Events;
using Theme;
using UnityEngine;
using UnityEngine.UI;


public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Button m_newGameButton;
    [SerializeField] private Button m_themeSelectionButton;
    [SerializeField] private ThemeSelectionPanelUI m_themeSelectionPanelUI;
    public void Awake()
    {
        m_newGameButton.onClick.AddListener(NewGameButtonOnClick);
        m_themeSelectionButton.onClick.AddListener(ThemeChangerButtonOnClick);
        m_themeSelectionPanelUI.Initialize();
    }

    public void Initialize(Action onNewButtonAction)
    {
        m_newGameButton.onClick.AddListener(onNewButtonAction.Invoke);
    }
    
    private void NewGameButtonOnClick()
    {
        m_themeSelectionButton.gameObject.SetActive(true);
    }
    
    private void ThemeChangerButtonOnClick()
    {
        m_themeSelectionPanelUI.Show();
    }
}
