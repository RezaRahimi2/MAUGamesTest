using System;
using UnityEngine;

[Serializable]
public struct CardModel
{
    [SerializeField]public bool FaceUp { get; private set; }
    [SerializeField]public CardSuit Suit { get; private set; }
    [SerializeField]public CardValue Value { get; private set; }

    public CardModel(CardSuit suit, CardValue cardValue)
    {
        Suit = suit;
        Value = cardValue;
        FaceUp = false;
    }

    public void SetFaceUP(bool faceUP)
    {
        FaceUp = faceUP;
    }
}