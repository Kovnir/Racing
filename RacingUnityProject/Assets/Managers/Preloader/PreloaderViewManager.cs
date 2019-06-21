using UnityEngine;
using Zenject;

public class PreloaderViewManager : MonoBehaviour
{
    [Inject] private PreloaderManager preloaderManager;
    
    void Start()
    {
        preloaderManager.Preloading();
    }
}
