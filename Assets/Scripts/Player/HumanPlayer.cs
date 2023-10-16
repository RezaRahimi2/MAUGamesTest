using System.Collections.Generic;
using Events;
using Player;
using UnityEngine;

public class HumanPlayer : IPlayer
{
    public bool IsBot{ get; set; }
    public int PlayerIndex { get; set; }
    public List<CardModel> Hand { get; set; }
    public CardModel PlayedCardModel { get; set; }

    public int Points { get; set; }

    [field:SerializeField]
    public bool IsReadyToPlay { get; set; }

    public void PlayCard()
    {
        EventManager.Broadcast(new OnWaitingForInputIsStartedEvent());
    }
}
