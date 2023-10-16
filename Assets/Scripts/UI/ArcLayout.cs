using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
public class ArcLayout : MonoBehaviour
{
    public bool m_isPlaceHolder;
    public float m_yOffset;
    public float radius = 5f; // Radius of the arc
    public float startAngle = 0f; // Starting angle in degrees
    public float endAngle = 180f; // Ending angle in degrees

    [SerializeField] private float startAngleInc;  
    [SerializeField] private float endAngleInc; 
    [SerializeField] private List<RectTransform> m_children;

#if UNITY_EDITOR
    void Update()
    {
        //if (m_children.Count != transform.childCount)
        if(!Application.isPlaying)
        {
            UpdateChildren();
            UpdateLayout();
        }
    }
#endif
    
    public RectTransform AddCard(RectTransform cardViewTransform)
    {
        cardViewTransform.SetParent(transform);
        UpdateChildren();
        UpdateLayout();
        return cardViewTransform;
    }
    
    public void RemoveCard()
    {
        UpdateChildren();
        UpdateLayout();
    }
    
    void UpdateChildren()
    {
        if (transform.childCount > 0)
        {
            m_children = transform.GetComponentsOnlyInChildren<RectTransform>().ToList();
        }
        else
        {
            m_children = new List<RectTransform>();
        }
    }

    public void UpdateLayout()
    {
        if (m_children.Count == 0)
            return;
        else if (m_children.Count == 1)
        {
            m_children[0].localEulerAngles = new Vector3(0, 0, 0);
            m_children[0].anchoredPosition = new Vector2(0, m_yOffset);
            return;
        }
        else if (m_children.Count <= 13)
        {
            startAngleInc = startAngle - (m_children.Count * 5);
            endAngleInc = endAngle + (m_children.Count * 5);
        }

        for (int i = 0; i < m_children.Count; i++)
        {
            // Calculate the angle of the current element
            float angle = Mathf.Lerp(startAngleInc, endAngleInc, (float)i / (m_children.Count - 1));

            // Convert the angle to radians
            float rad = Mathf.Deg2Rad * angle;

            // Calculate the position of the element
            float x = Mathf.Sin(rad) * radius;
            float y = Mathf.Cos(rad) * radius;

            if (m_isPlaceHolder || !Application.isPlaying)
            {
                m_children[i].anchoredPosition = new Vector2(x, y);
                m_children[i].localEulerAngles = new Vector3(0, 0, -angle);
            }
            else
            {
                // Set the position and rotation of the element
                m_children[i].DOAnchorPos(new Vector2(x, y),.5f);
                m_children[i].DOLocalRotate(new Vector3(0, 0, -angle), .5f);
            }
        }
    }
}