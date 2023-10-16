using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [field: SerializeField] public ThemeCustomizationData ThemeCustomizationData { get; private set; }
}