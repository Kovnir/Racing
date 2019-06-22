using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [InjectOptional(Id = "bindedObject")] private GameObject gameObject;
    [Inject] private DiContainer container;

    [Inject] private CarController car;
    [SerializeField] private StartCountdown startCountdown;

    [Inject] private SignalBus signalBus;

    private List<CheckPoint> checkPoints = new List<CheckPoint>();

    private int nextCheckpoint = 0;
    
    private void Awake()
    {
        signalBus.Subscribe<OnCheckpointAchievedSignal>(x =>
        {
            if (x.CheckPoint.Index == nextCheckpoint)
            {
                x.CheckPoint.Close();
                nextCheckpoint++;
            }
            else
            {
                signalBus.Fire<OnLoseCheckpointSignal>();                
            }
        });
        
        if (gameObject == null)
        {
            Debug.LogError("Null");
            return;
        }

        container.InstantiatePrefab(gameObject);
    }

    private void Start()
    {
        ProcessCheckpoints();
        StartCoroutine(StartSequence());
    }

    private void ProcessCheckpoints()
    {
        //sort
        checkPoints.Sort((c1, c2) => c1.Index.CompareTo(c2.Index));
        //validate
        int index = 0;
        foreach (var checkPoint in checkPoints)
        {
            if (checkPoint.Index != index)
            {
                Debug.LogError(
                    "Checkpoints configured wrong. You have spaces between checkpoints indexes or duplicated indexes!");
                return;
            }
            index++;
        }
        nextCheckpoint = 0;
    }


    public IEnumerator StartSequence()
    {
//        car.TakeControl();
        yield return startCountdown.Show();
//        car.ReturnControl();

    }

    public void RegisterCheckPoint(CheckPoint checkPoint)
    {
        checkPoints.Add(checkPoint);
    }
}
