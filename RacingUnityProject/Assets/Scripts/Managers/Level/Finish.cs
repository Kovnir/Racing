using System.Collections;
using System.Collections.Generic;
using Signals;
using UnityEngine;
using Zenject;

public class Finish : MonoBehaviour
{
    [Inject] private SignalBus signalBus;
    [Inject] private LevelManager levelManager;

    private void Awake()
    {
        levelManager.RegisterFinish(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            signalBus.Fire<OnFinishAchievedSignal>();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
