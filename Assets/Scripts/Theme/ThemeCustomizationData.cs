using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ThemeCustomizationData", menuName = "Theme Customization/Theme Customization Data")]
public class ThemeCustomizationData : ScriptableObject
{
    public string FrontCardSourceDirectory;
    public string FrontCardDestinationDirectory;
    public List<CustomizeCardFrontView> CradsFrontData;
    public List<CustomizeCardBackView> CardsBackData;
    public List<CustomizeBackground> BackgroundsData;
}
