using System.Collections.Generic;
using Events;
using Managers;
using Player;
using UnityEngine;

public class TableView : MonoBehaviour
{
    [SerializeField] private List<CardView> m_cardViews;
    [SerializeField] private List<Transform> m_cardPlaceInTableTransforms;

    public void AddCard(CardView cardView,int playerIndex)
    {
        m_cardViews.Add(cardView);
        cardView.transform.SetParent(m_cardPlaceInTableTransforms[playerIndex]);
    }

    public void CollectCardsToWinner(PlayerView playerView)
    {
        float delay = 0;
        EventManager.Broadcast(new OnCardAnimationIsStartedEvent());
        m_cardViews.ForEach(x=>
        {
            AnimationManager.Instance.CardsMoveAnimationWithCallback(x, playerView.transform.position,delay,1f, () =>
            {
                AnimationManager.Instance.CardsScaleAnimationWithCallback(x, Vector3.zero, () =>
                {
                    x.gameObject.SetActive(false);
                    m_cardViews.Remove(x);
                    EventManager.Broadcast(new OnCardAnimationIsFinishedEvent());
                });
            });
            delay += .1f;
        });
    }
    
    public Transform GetCardPlaceInTableByIndex(int index)
    {
        return m_cardPlaceInTableTransforms[index];
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        EventManager.Broadcast(new OnCardDraggedToTableEvent(){IsEnter = true});
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        EventManager.Broadcast(new OnCardDraggedToTableEvent(){IsEnter = false});
    }
}
