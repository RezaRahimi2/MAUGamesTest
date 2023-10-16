using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ThemeCustomizationData))]
public class ThemeCustomizationDataEditor : UnityEditor.Editor
{
    private ThemeCustomizationData m_themeCustomizationData;

    public override VisualElement CreateInspectorGUI()
    {
        m_themeCustomizationData = target as ThemeCustomizationData;
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Rename"))
        {
            string[] files = Directory.GetFiles(m_themeCustomizationData.FrontCardSourceDirectory);
            
            string[] cardSuits = Enum.GetNames(typeof(CardSuit));
            string patternSuit = string.Join("|", cardSuits.Select(w => Regex.Escape(w)));
            Regex regexSuit = new Regex(patternSuit, RegexOptions.IgnoreCase);
            
            string[] cardValue = Enum.GetNames(typeof(CardValue));
            string patternValue = string.Join("|", cardValue.Select(w => Regex.Escape(w)));
            Regex regexValueName = new Regex(patternValue, RegexOptions.IgnoreCase);
            
            Regex regexValue = new Regex(@"(\d+)(?!.*\d)");

            for (var i = 0; i < files.Length; i++)
            {
                string value = regexValueName.Match(files[i]).Value;
                if (!string.IsNullOrEmpty( value) && Enum.TryParse(typeof(CardValue),value , true, out var result))
                {
                    value = result.ToString();
                }
                else
                {
                    value = ((CardValue)int.Parse(regexValue.Match(files[i]).Value)).ToString();
                }
                string newFileName = Enum.Parse(typeof(CardSuit),regexSuit.Match(files[i]).Value,true) + "_" + value + Path.GetExtension(files[i]);
                System.IO.File.Copy(files[i],Path.Combine(m_themeCustomizationData.FrontCardDestinationDirectory,newFileName));
            }
            AssetDatabase.Refresh();
        }
    }
}