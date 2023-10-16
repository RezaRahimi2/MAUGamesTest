using System;
using Events;
using Managers;
using Theme;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
   [SerializeField] private Button m_blocker;
   [SerializeField] private Button m_menuButton;
   [SerializeField] private MenuPanel m_menuPanel;
   private void Awake()
   {
      m_blocker.onClick.AddListener(HideMenu);
      m_menuButton.onClick.AddListener(ShowMenu);
      m_menuPanel.Initialize(OnNewGame);
   }

   private void OnNewGame()
   {
      GameLoader.Instance.LoadGame();
      HideMenu();
   }

   public void ShowMenu()
   {
      m_blocker.gameObject.SetActive(true);
      AnimationManager.Instance.UIFadeAnimation(m_blocker.image, .4f);
      AnimationManager.Instance.MoveUIAnimationWithCallback(((RectTransform)m_menuPanel.transform), Vector3.zero,0,.5f);
   }

   public void HideMenu()
   {
      AnimationManager.Instance.UIFadeAnimation(m_blocker.image, 0, () =>
      {
         m_blocker.gameObject.SetActive(false);
      });
      AnimationManager.Instance.MoveUIAnimationWithCallback(((RectTransform)m_menuPanel.transform),
         new Vector3(-((RectTransform)m_menuPanel.transform).rect.width,0,0) ,0,.5f);
   }
}
