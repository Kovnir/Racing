using UnityEngine;
using Zenject;

public class LevelViewManager : MonoBehaviour
{
    [InjectOptional(Id = "bindedObject")] private GameObject gameObject;
    [Inject] private DiContainer container;
    
    private void Awake()
    {
        if (gameObject == null)
        {
            Debug.LogError("Null");
            return;
        }

        container.InstantiatePrefab(gameObject);
    }
}
