using Player;
using Theme;
using UnityEngine;

    public class PrefabsManager : Singleton<PrefabsManager>
    {
        [Header("Game")]
        public PlayerView HumanPlayerView;
        public PlayerView AIPlayerView;
        
        [Header("Theme")]
        public CardFrontThemeSelectionElement  CardFrontElementPrefab;
        public CardBackThemeSelectionElement CardBackElementPrefab;
        public BackgroundThemeSelectionElement BackgroundElementPrefab;
    }
