using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class GameLoader : Singleton<GameLoader>
{
    [SerializeField] private GameObject m_gamePrefab;
    [SerializeField] private GameObject m_gameInstance;
    
    public void LoadGame()
    {
        if(GameController.Instance != null)
            GameController.Instance.CancellationToken.Cancel();
        
        DOTween.KillAll();
        EventManager.CleanUp();
        if(m_gameInstance)
            Destroy(m_gameInstance.gameObject);
        
        m_gameInstance = Instantiate(m_gamePrefab);
    }
    
}
