using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Card.Editor
{
    [CustomEditor(typeof(CardView))]
    public class CardViewEditor : UnityEditor.Editor
    {
        private CardView m_cardView;
        
        public override VisualElement CreateInspectorGUI()
        {
            m_cardView = target as CardView;
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Test Flip Animation"))
            {
                m_cardView.SetFaceUp(!m_cardView.IsFaceUp());
            }
        }
    }
}