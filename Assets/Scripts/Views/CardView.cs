using System;
using DG.Tweening;
using Events;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [field: SerializeField] public CardModel CardModel { get; private set; }

    [field:SerializeField]
    public Image CardFront { get; private set; }
    
    [field:SerializeField]
    public Image CardBack { get; private set; }
    
    private Tweener m_flipAnim = null;
    [field:SerializeField]public Canvas Canvas { get; private set; }

    private void Start()
    {
        EventManager.Subscribe<OnChangeCardFrontEvent>(UpdateCardFront);
        EventManager.Subscribe<OnChangeCardBackEvent>(UpdateCardBack);
    }

    public void Initialize(CardModel cardModel, Canvas canvas)
    {
        // Display the card here
        CardModel = cardModel;
        // Get the sprite for the card
        Canvas = canvas;

        name = $"{cardModel.Suit}-{cardModel.Value}";

        UpdateCardFront(new OnChangeCardFrontEvent());
        UpdateCardBack(new OnChangeCardBackEvent());
    }

    private void UpdateCardFront(OnChangeCardFrontEvent changeCardFrontEvent)
    {
        Sprite cardSprite = SpriteManager.Instance.GetCardSprite(CardModel.Suit, CardModel.Value);
        CardFront.sprite = cardSprite;
    }

    private void UpdateCardBack(OnChangeCardBackEvent changeCardBackEvent)
    {
        CardBack.sprite = SpriteManager.Instance.CardBack;
    }
    
    public void SetFaceUp(bool value)
    {
        if (CardModel.FaceUp != value)
        {
            //Finish all steps of potential previous flip anim
            while (m_flipAnim != null)
            {
                m_flipAnim.Complete();
            }

            m_flipAnim = AnimationManager.Instance.CardsScaleAnimationWithCallback(this, new Vector3(0, 1, 1), ()=>
            {
                CardBack.gameObject.SetActive(!value);
                CardFront.gameObject.SetActive(value);
                m_flipAnim = AnimationManager.Instance.CardsScaleAnimationWithCallback(this, new Vector3(1, 1, 1), ()=>
                {
                    m_flipAnim = null;
                });
                
            });
        }

        CardModel.SetFaceUP(value);
    }

#if UNITY_EDITOR
    public bool IsFaceUp()
    {
        return CardModel.FaceUp;
    }
#endif

}
