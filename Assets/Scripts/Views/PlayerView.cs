using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Managers;
using UI;
using UnityEngine;

namespace Player
{
    public sealed class PlayerView : MonoBehaviour
    {
        [field: SerializeField] public bool IsPlayerTurn { get; private set; }
        [field: SerializeField] public bool IsBot => m_player.IsBot;

        [field: SerializeField] public IPlayer m_player { get; private set; }
        [SerializeField] private PlayerUI m_playerUI;
        [field: SerializeField] public Transform ReceiveCardTransform { get; private set; }

        [SerializeField] private Transform m_cardsParent;
        [SerializeField] private ArcLayout m_cardsArcLayout;

        [SerializeField] private ArcLayout m_cardsPlaceHolderArcLayout;
        [SerializeField] private Transform m_cardPlaceHolder;
        [SerializeField] private Transform m_cardsPlaceHolderParent;

        [field: SerializeField] public List<CardView> CardView { get; protected set; }

        public void Initialize(IPlayer player)
        {
            m_player = player;
            CardView = new List<CardView>();
        }

        public void OnPlayCard(CardModel cardModel)
        {
            m_player.PlayedCardModel = cardModel; 
            m_cardsArcLayout.RemoveCard();
            CardView.Remove(GetCardViewByCardModel(cardModel));
        }

        public void ReceivedCard(CardView cardView)
        {
            RectTransform cardViewPlaceHolder =
                m_cardsPlaceHolderArcLayout.AddCard((RectTransform)Instantiate(m_cardPlaceHolder,
                    m_cardsPlaceHolderParent));
            cardView.transform.SetParent(m_cardsArcLayout.transform);
            if (!IsBot)
            {
                cardView.SetFaceUp(true);
            }
            else
                AnimationManager.Instance.CardsScaleAnimationWithCallback(cardView,
                    m_cardsArcLayout.transform.localScale, null);

            AnimationManager.Instance.CardsMoveAnimationWithCallback(cardView, cardViewPlaceHolder.position, 0, null);
            
            AnimationManager.Instance.CardsRotateAnimationWithCallback(cardView,cardViewPlaceHolder.transform.rotation.eulerAngles,
                () =>
                {
                    m_cardsArcLayout.AddCard((RectTransform)cardView.transform);
                });
        }

        public CardView GetCardViewByCardModel(CardModel cardModel)
        {
            return CardView.Find(x => Equals(x.CardModel, cardModel));
        }

        public void AddScore()
        {
            m_playerUI.SetScore(m_player.Points);
        }

        public void OnPlayerTurn(bool isTurn)
        {
            m_player.IsReadyToPlay = isTurn;
            if (isTurn)
                m_playerUI.ShowIndicator();
            else
                m_playerUI.HideIndicator();
        }
    }
}