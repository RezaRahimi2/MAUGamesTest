using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteManager : Singleton<SpriteManager>
{
    [SerializeField] private CardFrontsEnum m_cardFrontsEnum;
    public SpriteAtlas FrontCardAtlas; // Assign in the inspector

    private Dictionary<string, Sprite> m_cardSprites;
    
    [field:SerializeField]
    public Sprite CardBack { get; private set; }
    
    [field:SerializeField]
    public Sprite Background { get; private set; }
       
    void Awake()
    {
        m_cardFrontsEnum = CardFrontsEnum.Main;
        LoadFrontCardsSprites();

        EventManager.Subscribe<OnChangeCardFrontEvent>(OnChangeCardFront);
        EventManager.Subscribe<OnChangeCardBackEvent>(OnChangeCardBack);
        EventManager.Subscribe<OnChangeBackgroundEvent>(OnChangeBackground);
    }

    private void LoadFrontCardsSprites()
    {
        // Load all sprites into the dictionary
        m_cardSprites = new Dictionary<string, Sprite>();
        foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
        {
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                string spriteName = $"{suit}_{value}";
                Sprite sprite = FrontCardAtlas.GetSprite(spriteName);
                if (sprite != null)
                {
                    m_cardSprites.Add(spriteName, sprite);
                }
                else
                {
                    Debug.LogError($"Sprite not found: {spriteName}");
                }
            }
        }
    }

    private void OnChangeCardFront(OnChangeCardFrontEvent changeCardFrontEvent)
    {
        FrontCardAtlas = DataManager.Instance.ThemeCustomizationData.CradsFrontData.Find(x=> x.Type == changeCardFrontEvent.CardFrontEnum).SpriteAtlas;
        if (m_cardFrontsEnum != changeCardFrontEvent.CardFrontEnum)
        {
            LoadFrontCardsSprites();
            m_cardFrontsEnum = changeCardFrontEvent.CardFrontEnum;
        }

    }
    
    private void OnChangeCardBack(OnChangeCardBackEvent changeCardBackEvent)
    {
        CardBack = DataManager.Instance.ThemeCustomizationData.CardsBackData.Find(x=> x.Type == changeCardBackEvent.CardBackEnum).PreviewSprite;
    }

    private async void OnChangeBackground(OnChangeBackgroundEvent onChangeBackgroundEvent)
    {
        Background = await DataManager.Instance.ThemeCustomizationData.BackgroundsData.Find(x=> x.Type == onChangeBackgroundEvent.BackgroundsEnum).LoadSprite();
        EventManager.Broadcast(new OnBackgroundLoadedEvent());
    }
    
    public Sprite GetCardSprite(CardSuit cardSuit, CardValue cardValue)
    {
        // Construct the sprite name based on the Suit and CardValue
        string spriteName = $"{cardSuit}_{cardValue}";

        // Get the sprite from the dictionary
        Sprite sprite;
        if (!m_cardSprites.TryGetValue(spriteName, out sprite))
        {
            Debug.LogError($"Sprite not found: {spriteName}");
        }

        return sprite;
    }
    
    
}
