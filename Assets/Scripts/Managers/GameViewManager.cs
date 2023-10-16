using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using Managers;
using Player;
using UnityEngine;

public class GameViewManager : MonoBehaviour
{
    [SerializeField] private Dictionary<IPlayer, PlayerView> playerViews; // List to hold all PlayerView instances
    [SerializeField] private List<Transform> playersParent; // List to hold all PlayerView instances

    [SerializeField] private TableView m_tableView;
    [SerializeField] private DealerView m_dealerView;
    [SerializeField] private CardView m_cardView;
    [SerializeField] private Canvas m_canvas;

    private void Awake()
    {
        m_canvas.worldCamera = Camera.main;
        EventManager.Subscribe<OnNewGameEvent>(Initialize);
        EventManager.Subscribe<OnCardsInitializedEvent>(OnCardsInitialized);
        EventManager.Subscribe<OnStartGameEvent>(OnStartGame);
        EventManager.Subscribe<OnDealCardsEvent>(OnCardsDeal);
        EventManager.Subscribe<OnSetPlayerTurnEvent>(OnPlayerTurn);
        EventManager.Subscribe<OnPlayedCardEvent>(OnPlayedCard);
        EventManager.Subscribe<OnRoundWinnerEvent>(OnRoundWinner);
    }

    private void Initialize(OnNewGameEvent onNewGameEvent)
    {
        if(playerViews != null && playerViews.Count > 0)
            foreach (PlayerView playerViewsValue in playerViews.Values)
            {
                Destroy(playerViewsValue.gameObject);
            }
    }

    private void OnStartGame(OnStartGameEvent onStartGame)
    {
        playerViews = new Dictionary<IPlayer, PlayerView>();
        for (var i = 0; i < onStartGame.Players.Count; i++)
        {
            PlayerView playerView;
            if (onStartGame.Players[i].IsBot)
            {
                playerView = Instantiate<PlayerView>(PrefabsManager.Instance.AIPlayerView, playersParent[i]);
            }
            else
            {
                playerView = Instantiate<PlayerView>(PrefabsManager.Instance.HumanPlayerView, playersParent[i]);
            }

            playerView.Initialize(onStartGame.Players[i]);
            playerViews.Add(onStartGame.Players[i], playerView);
        }
    }

    private void OnCardsInitialized(OnCardsInitializedEvent onCardsInitializedEvent)
    {
        //GameController.Instance.Deck.ForEach(x=> );
        DragAndDrop dragAndDropComponent;

        for (int i = 0; i < 13; i++)
        {
            for (var i1 = 0; i1 < onCardsInitializedEvent.Players.Count; i1++)
            {
                CardView cardView = Instantiate(m_cardView, m_dealerView.CardPlace);
                cardView.Initialize(onCardsInitializedEvent.Players[i1].Hand[i], m_canvas);
                if (!onCardsInitializedEvent.Players[i1].IsBot)
                {
                    dragAndDropComponent = cardView.gameObject.AddComponent<DragAndDrop>();
                    dragAndDropComponent.Initialize((RectTransform)cardView.transform,
                        new PlayerCardData(onCardsInitializedEvent.Players[i1], cardView.CardModel, false));
                }

                playerViews[onCardsInitializedEvent.Players[i1]].CardView.Add(cardView);
            }
        }
    }

    private void OnCardsDeal(OnDealCardsEvent onDealCardsEvent)
    {
        m_dealerView.Initialize(playerViews.Values.Select(x => x).ToList());
        m_dealerView.OnDeal(onDealCardsEvent.PlayersIndexBasedDealer);
    }

    private void OnPlayerTurn(OnSetPlayerTurnEvent onSetPlayerTurnEvent)
    {
        foreach (PlayerView playerViewsValue in playerViews.Values)
        {
            if (playerViewsValue.m_player != onSetPlayerTurnEvent.Player)
                playerViewsValue.OnPlayerTurn(false);
        }

        playerViews[onSetPlayerTurnEvent.Player].OnPlayerTurn(true);
    }

    private void OnPlayedCard(OnPlayedCardEvent onPlayedCard)
    {
        string tweeName =
            $"player{onPlayedCard.PlayerCardData.Player}/card{onPlayedCard.PlayerCardData.CardModel}";

        CardView cardView = playerViews[onPlayedCard.PlayerCardData.Player]
            .GetCardViewByCardModel(onPlayedCard.PlayerCardData.CardModel);

        EventManager.Broadcast(new OnCardAnimationIsStartedEvent());

        m_tableView.AddCard(cardView,onPlayedCard.PlayerCardData.Player.PlayerIndex);

        AnimationManager.Instance.CardsScaleAnimationWithCallback(cardView, Vector3.one, () =>
        {
            if (onPlayedCard.PlayerCardData.IsBot)
            {
                cardView.SetFaceUp(true);
            }
        });

        playerViews[onPlayedCard.PlayerCardData.Player].GetCardViewByCardModel(onPlayedCard.PlayerCardData.CardModel)
            .transform
            .DOMove(m_tableView.GetCardPlaceInTableByIndex(onPlayedCard.PlayerCardData.Player.PlayerIndex).position, 1)
            .SetTarget(tweeName)
            .OnComplete(() => { EventManager.Broadcast(new OnCardAnimationIsFinishedEvent()); });

        if (onPlayedCard.PlayerCardData.IsBot)
        {
            playerViews[onPlayedCard.PlayerCardData.Player]
                .GetCardViewByCardModel(onPlayedCard.PlayerCardData.CardModel).transform.DORotate(Vector3.zero, .5f);
        }

        playerViews[onPlayedCard.PlayerCardData.Player].OnPlayCard(onPlayedCard.PlayerCardData.CardModel);
    }

    private void OnRoundWinner(OnRoundWinnerEvent onRoundWinnerEvent)
    {
        EventManager.Broadcast(new OnWaitingForUIChangingStartedEvent());
        m_tableView.CollectCardsToWinner(playerViews[onRoundWinnerEvent.Player]);
        playerViews[onRoundWinnerEvent.Player].AddScore();
        AnimationManager.Instance.DelayedCall(() => EventManager.Broadcast(new OnWaitingForUIChangingFinishedEvent()), 1);
    }
}