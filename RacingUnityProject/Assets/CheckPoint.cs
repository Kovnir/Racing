using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CheckPoint : MonoBehaviour
{
    [Inject] private LevelManager levelManager;
    [Inject] private SignalBus signalBus;

    [SerializeField] private int index;
    public int Index
    {
        get { return index; }
    } 
    
    private void Awake()
    {
        levelManager.RegisterCheckPoint(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            signalBus.Fire(new OnCheckpointAchievedSignal(this));
        }
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
}
