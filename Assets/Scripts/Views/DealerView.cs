using System;
using System.Collections.Generic;
using Events;
using Managers;
using Player;
using UnityEngine;

public class DealerView : MonoBehaviour
{
    [SerializeField] private List<PlayerView> m_playersView;
    [field:SerializeField]public Transform CardPlace { get; private set; }

    public void Initialize(List<PlayerView> playerViews)
    {
        m_playersView = playerViews;
    }

    public void OnDeal(List<int> playersIndex)
    {
        float delay = 0;
        EventManager.Broadcast(new OnCardAnimationIsStartedEvent());
        Action callBack = ()=> EventManager.Broadcast(new OnCardAnimationIsFinishedEvent());

        int cardIndex = 0;
        for (int i = 0; i < 13; i++)
        {
            for (var i1 = 0; i1 < m_playersView.Count; i1++)
            {
                PlayerView playerView = m_playersView[playersIndex[i1]];
                cardIndex++;
                var cardViewIndex = i;
                var deckCardIndex = cardIndex;
                AnimationManager.Instance.CardsMoveAnimationWithCallback(playerView.CardView[i],playerView.ReceiveCardTransform.position,
                    delay,()=>  playerView.ReceivedCard(playerView.CardView[cardViewIndex])
                    , () => deckCardIndex == 52,
                    callBack);
                delay += .25f;

            }

            delay += .5f;
        }
    }
}
