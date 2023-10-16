using System;
using UnityEngine;

[Serializable]
public struct PlayerCardData
{
    [SerializeField]
    public IPlayer Player;
    [SerializeField]
    public CardModel CardModel;
    [SerializeField]
    public bool IsBot;

    public PlayerCardData(IPlayer player, CardModel cardModel, bool isBot)
    {
        Player = player;
        CardModel = cardModel;
        IsBot = isBot;
    }
}