using System.Collections;
using UnityEngine;
using Zenject;

public class LevelViewManager : MonoBehaviour
{
    [InjectOptional(Id = "bindedObject")] private GameObject gameObject;
    [Inject] private DiContainer container;

    [Inject] private CarController car;
    [SerializeField] private StartCountdown startCountdown;
    
    private void Awake()
    {
        StartCoroutine(StartSequence());
        
        
        if (gameObject == null)
        {
            Debug.LogError("Null");
            return;
        }

        container.InstantiatePrefab(gameObject);
    }

    public IEnumerator StartSequence()
    {
        car.TakeControl();
        yield return startCountdown.Show();
        car.ReturnControl();

    }
}
