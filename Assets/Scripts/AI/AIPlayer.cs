using System.Collections.Generic;
using Events;
using UnityEngine;

public sealed class AIPlayer : IPlayer
{
    public bool IsBot { get; set; }
    public int PlayerIndex { get; set; }
    public List<CardModel> Hand { get; set; }
    public CardModel PlayedCardModel { get; set; }
    public int Points { get; set; }

    [field:SerializeField]
    public bool IsReadyToPlay { get; set; }

    public void PlayCard()
    {
        PlayedCardModel = Hand[Random.Range(0, Hand.Count - 1)];
        
        // Play the selected card
        EventManager.Broadcast(new OnPlayedCardEvent( new PlayerCardData(this, PlayedCardModel ,true)));
        Hand.Remove(PlayedCardModel);

    }
}
