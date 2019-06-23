using System.Collections;
using System.Collections.Generic;
using Signals;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject] private LevelSettings levelSettings;
    [Inject] private DiContainer container;

    [SerializeField]
    private CarController carPrefab;
    private CarController car;
    [SerializeField] private StartCountdown startCountdown;

    [Inject] private SignalBus signalBus;
    
    private List<CheckPoint> checkPoints = new List<CheckPoint>();
    private Finish finish;
    
    private int nextCheckpoint = 0;
    private bool levelEnded;
    
    private void Awake()
    {
        car = container.InstantiatePrefab(carPrefab).GetComponent<CarController>();
        container.Bind<CarController>().FromInstance(car).AsSingle();
        signalBus.Subscribe<OnCheckpointAchievedSignal>(x =>
        {
            if (levelEnded)
            {
                return;
            }

            if (x.CheckPoint.Index == nextCheckpoint)
            {
                x.CheckPoint.Close();
                nextCheckpoint++;
                signalBus.Fire<OnTakeCheckpointSignal>();
                if (checkPoints.Count > nextCheckpoint + 1)
                {
                    checkPoints[nextCheckpoint].ShowStraight();
                    checkPoints[nextCheckpoint + 1].Show();
                }
                else
                {
                    finish.Show();
                }
            }
            else
            {
                signalBus.Fire<OnLoseCheckpointSignal>();                
            }
        });
        signalBus.Subscribe<OnFinishAchievedSignal>(() =>
        {
            if (levelEnded)
            {
                return;
            }
            if (nextCheckpoint == checkPoints.Count)
            {
                signalBus.Fire<OnLevelFinishedSignal>();
            }
            else
            {
                signalBus.Fire<OnLoseCheckpointSignal>();                
            }
        });
        signalBus.Subscribe<OnLevelFailedSignal>(() =>
        {
            levelEnded = true;
            car.TakeControl();
        });
        signalBus.Subscribe<OnLevelFinishedSignal>(() =>
        {
            levelEnded = true;
            car.TakeControl();
        });
    }

    private void Start()
    {
        ProcessCheckpoints();
//        signalBus.Fire(new UpdateCheckpointsHudSignal(0, checkPoints.Count));
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

        if (finish == null)
        {
            Debug.LogError("Finish not registred!");
        }
        
        for (int i = 2; i < checkPoints.Count; i++)
        {
            checkPoints[i].Hide();
        }
        checkPoints[0].ShowStraight();

        if (checkPoints.Count > 1)
        {
            finish.Hide();
        }
    }


    public IEnumerator StartSequence()
    {
        car.TakeControl();
        yield return startCountdown.Show();
        car.ReturnControl();
        signalBus.Fire<OnRaceStartSignal>();
    }

    public void RegisterCheckPoint(CheckPoint checkPoint)
    {
        checkPoints.Add(checkPoint);
    }
    public void RegisterFinish(Finish finish)
    {
        if (this.finish != null)
        {
            Debug.LogError("There is more than 1 finish in the level");
            return;
        }
        this.finish = finish;
    }

    public int GetCheckpointsCount()
    {
        return checkPoints.Count;
    }
}
