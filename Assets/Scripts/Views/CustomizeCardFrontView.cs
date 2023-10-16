using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;

[Serializable]
public class CustomizeCardFrontView
{
    public CardFrontsEnum Type;
    public Sprite PreviewSprite;
    public SpriteAtlas SpriteAtlas;
}
