using System.Collections.Generic;
using Managers.Level.Ghost;
using UnityEngine;
using Zenject;

public class GhostRecorder : MonoBehaviour
{
    private CarController car;

    public List<GhostData> records = new List<GhostData>();

    [Inject] private DiContainer container;

    private void Awake()
    {
        container.Bind<GhostRecorder>().FromInstance(this).AsSingle();
    }

    // Start is called before the first frame update
    void Start()
    {
        car = container.Resolve<CarController>();
        time = Time.time;
    }

    private float time;
    private bool finished;
    private void Update()
    {
        if (finished)
        {
            return;
        }
        if (Time.time - time > 0.5f)
        {
            records.Add(new GhostData
            {
                Position = car.transform.position,
                Rotation = car.transform.rotation.eulerAngles
            });
            time -= Time.time;
        }
    }

    public List<GhostData> ShotRecording()
    {
        finished = true;
        return records;
    }
}
