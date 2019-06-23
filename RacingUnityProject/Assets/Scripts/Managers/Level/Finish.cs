using System.Collections;
using System.Collections.Generic;
using Signals;
using UnityEngine;
using Zenject;

public class Finish : MonoBehaviour
{
    [Inject] private SignalBus signalBus;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            signalBus.Fire<OnFinishAchievedSignal>();
        }
    }
}
