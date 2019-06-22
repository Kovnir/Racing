using System.Collections;
using System.Collections.Generic;
using Singals;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    //optional for make possible to run scene without all another game
    [InjectOptional] private LevelSettings levelSettings;
    [Inject] private DiContainer container;

    [SerializeField]
    private CarController car;
    [SerializeField] private StartCountdown startCountdown;

    [Inject] private SignalBus signalBus;

    private List<CheckPoint> checkPoints = new List<CheckPoint>();

    private int nextCheckpoint = 0;
    
    private void Awake()
    {
        var carPrefab = container.InstantiatePrefab(car);
        var comp = carPrefab.GetComponent<CarController>();
        container.Bind<CarController>().FromInstance(comp);
        
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
        signalBus.Fire<OnRaceStartSignal>();
    }

    public void RegisterCheckPoint(CheckPoint checkPoint)
    {
        checkPoints.Add(checkPoint);
    }
}
