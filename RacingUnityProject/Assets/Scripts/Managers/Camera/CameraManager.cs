using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    
    [Inject] private DiContainer container;

    private CarController car;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    void Start()
    {
        car = container.Resolve<CarController>();
        cinemachineVirtualCamera.Follow = car.transform;
        cinemachineVirtualCamera.LookAt = car.transform;
        //do it here because GameHudViewManager can be created before LevelManager, which bind car;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
