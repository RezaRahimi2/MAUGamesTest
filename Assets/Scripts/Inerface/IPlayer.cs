using System.Collections.Generic;

public interface IPlayer
{
    public bool IsBot { get; set; }
    public int PlayerIndex { get; set; }
    public List<CardModel> Hand { get; set; }
    public CardModel PlayedCardModel { get; set; }

    public int Points { get; set; }
    public bool IsReadyToPlay { get; set; }
    
    public void PlayCard();
}