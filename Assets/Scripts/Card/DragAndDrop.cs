using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
    [field: SerializeField] public BoxCollider2D BoxCollider2D { get; private set; }
    [SerializeField] private RectTransform m_rectTransform;
    [SerializeField] private PlayerCardData m_playerCardData;
    [SerializeField] private bool m_isInTableArea;

    private Vector3 m_lastPosition;
    private Vector3 m_lastRotation;
    private Vector3 m_lastScale;

    private Vector2 offset;

    public void Initialize(RectTransform rectTransform, PlayerCardData playerCardData)
    {
        m_rectTransform = rectTransform;
        m_playerCardData = playerCardData;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.isKinematic = true;
        Rigidbody2D.simulated = true;

        BoxCollider2D = GetComponent<BoxCollider2D>();
        var rect = rectTransform.rect;
        BoxCollider2D.size = new Vector2(rect.width, rect.height);
        BoxCollider2D.offset = new Vector2(0, -(rect.height / 2));
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the rectTransform to the current cursor position
        Vector2 cursorPos = eventData.position;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(cursorPos.x,
            cursorPos.y + m_rectTransform.rect.height / 2, Camera.main.nearClipPlane));
        m_rectTransform.position = worldPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerClick = null;
        if (m_playerCardData.Player.IsReadyToPlay)
        {
            m_isInTableArea = false;
            m_lastPosition = transform.localPosition;
            m_lastRotation = transform.localRotation.eulerAngles;
            m_lastScale = transform.localScale;

            // Update the position of the rectTransform to the current cursor position
            Vector2 cursorPos = eventData.position;

            // Calculate the center of the rectTransform in world coordinates
            Vector3 centerPos = m_rectTransform.position + new Vector3(0, m_rectTransform.rect.height / 2, 0);
            Vector3 worldPos =
                Camera.main.ScreenToWorldPoint(new Vector3(cursorPos.x, centerPos.y, Camera.main.nearClipPlane));

            m_rectTransform.position = worldPos;

            m_rectTransform.DORotate(Vector3.zero, .5f).SetTarget($"{name}_OnBeginDrag");
            m_rectTransform.DOScale(Vector3.one, .5f).SetTarget($"{name}_OnBeginDrag");
            //SoundManager.Instance.Play(SoundsEnum.CARD_DRAG_BEGIN);
            EventManager.Subscribe<OnCardDraggedToTableEvent>(OnCardDraggedToTable);
        }
        else
            eventData.pointerDrag = null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EventManager.Unsubscribe<OnCardDraggedToTableEvent>(OnCardDraggedToTable);
        EventManager.Broadcast(new OnCardEndDragEvent(m_playerCardData));

        if (m_isInTableArea)
        {
            EventManager.Broadcast(new OnPlayedCardEvent()
            {
                PlayerCardData = m_playerCardData
            });
            EventManager.Broadcast(new OnWaitingForInputIsFinishedEvent());
        }
        else
        {
            m_rectTransform.DOLocalMove(m_lastPosition, .5f).SetTarget($"{name}_OnEndDrag");
            m_rectTransform.DOLocalRotate(m_lastRotation, .5f).SetTarget($"{name}_OnEndDrag");
            m_rectTransform.DOScale(m_lastScale, .5f).SetTarget($"{name}_OnEndDrag");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.Broadcast(new OnCardTapEvent(m_playerCardData));
    }

    private void OnCardDraggedToTable(OnCardDraggedToTableEvent draggedToTableEvent)
    {
        m_isInTableArea = draggedToTableEvent.IsEnter;
    }
}