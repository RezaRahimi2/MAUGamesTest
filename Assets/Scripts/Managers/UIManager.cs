using Events;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Image m_bacgroundUI;
        [SerializeField] private CanvasGroup m_startGameUI;
        [SerializeField] private RoundUI m_roundUI;
        [SerializeField] private WinnerUI m_winnerUI;

        private void Awake()
        {
            m_bacgroundUI = GameObject.Find("BackgroundCanvas/Background").GetComponent<Image>();
            EventManager.Subscribe<OnStartGameEvent>(OnStartGame);
            EventManager.Subscribe<OnRoundStartEvent>(OnRoundStart);
            EventManager.Subscribe<OnWinnerEvent>(OnWinner);
            EventManager.Subscribe<OnBackgroundLoadedEvent>(SetBackground);
        }

        private void SetBackground(OnBackgroundLoadedEvent onBackgroundLoadedEvent)
        {
            m_bacgroundUI.sprite = SpriteManager.Instance.Background;
        }
        
        private void OnStartGame(OnStartGameEvent onStartGameEvent)
        {
            AnimationManager.Instance.UIFadeInOutAnimation(m_startGameUI);
        }

        private void OnRoundStart(OnRoundStartEvent roundStartEvent)
        {
            m_roundUI.SetText(roundStartEvent.RoundNumber);
            AnimationManager.Instance.UIFadeInOutAnimation(m_roundUI.CanvasGroup);
        }
        
        private void OnWinner(OnWinnerEvent onWinnerEvent)
        {
            m_winnerUI.SetText(onWinnerEvent.PlayerIndex);
            AnimationManager.Instance.UIFadeAnimation(m_winnerUI.CanvasGroup,1);
        }
    }
}
