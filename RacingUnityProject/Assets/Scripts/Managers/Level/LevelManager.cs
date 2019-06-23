using System.Collections;
using System.Collections.Generic;
using Signals;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject] private LevelSettings levelSettings;
    [Inject] private DiContainer container;
    [Inject] private CameraManager cameraManager;
    [Inject] private SignalBus signalBus;
    
    [SerializeField]
    private CarController carPrefab;
    private CarController car;
    
    [SerializeField] private StartCountdown startCountdown;

    
    private List<CheckPoint> checkPoints = new List<CheckPoint>();
    private Finish finish;
    private SpawnPoint spawnPoint;
    
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
                if (nextCheckpoint + 1 < checkPoints.Count)
                {
                    checkPoints[nextCheckpoint].ShowStraight();
                    checkPoints[nextCheckpoint + 1].Show();
                    cameraManager.RemoveTarget(checkPoints[nextCheckpoint-1].transform);
                    cameraManager.AddTarget(checkPoints[nextCheckpoint+1].transform);
                }
                else
                {
                    if (nextCheckpoint + 1 == checkPoints.Count)
                    {
                        finish.Show();
                        checkPoints[nextCheckpoint].ShowStraight();
                        cameraManager.RemoveTarget(checkPoints[nextCheckpoint-1].transform);
                        cameraManager.AddTarget(finish.transform);
                    }
                    else
                    {
                        cameraManager.RemoveTarget(checkPoints[nextCheckpoint-1].transform);
                    }
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
        car.transform.position = spawnPoint.transform.position;
        car.transform.rotation = spawnPoint.transform.rotation;
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

        if (checkPoints.Count < 2)
        {
            Debug.LogError(
                "You have less then 2 checkpoints! Add more!");
            return;
        }
        if (finish == null)
        {
            Debug.LogError("Finish not registred!");
        }
        
        for (int i = 2; i < checkPoints.Count; i++)
        {
            checkPoints[i].Hide();
        }
        checkPoints[0].ShowStraight();
        cameraManager.AddTarget(checkPoints[0].transform);
        cameraManager.AddTarget(checkPoints[1].transform);
        finish.Hide();
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
            Debug.LogError("There is more than 1 finish in the level!");
            return;
        }
        this.finish = finish;
    }

    public int GetCheckpointsCount()
    {
        return checkPoints.Count;
    }

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if (this.spawnPoint != null)
        {
            Debug.LogError("There is more than 1 spawn points in the level!");
            return;
        }
        this.spawnPoint = spawnPoint;
    }
}
