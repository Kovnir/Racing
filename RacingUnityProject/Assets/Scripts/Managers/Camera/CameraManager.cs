using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Signals;
using UnityEngine;
using UnityEngine.PostProcessing;
using Zenject;

public class CameraManager : MonoBehaviour
{    
    [Inject] private DiContainer container;
    [Inject] private SignalBus bus;
    [Inject] private PlayerProfileManager playerProfileManager;

    private CarController car;
    private PostProcessingBehaviour postProcessingBehaviour;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        //setup postprocessing
        postProcessingBehaviour = Camera.main.GetComponent<PostProcessingBehaviour>();
        UpdatePostProcessing();
        bus.Subscribe<OnPostProcessingSettingsChangedSignal>(UpdatePostProcessing);
}

    private void UpdatePostProcessing()
    {
        postProcessingBehaviour.enabled = playerProfileManager.GetPostProcessingState();
    }

    private void Start()
    {
        //do it here because GameHudViewManager can be created before LevelManager, which bind car;
        car = container.Resolve<CarController>();
        //setup cinemachine
        cinemachineVirtualCamera.Follow = car.transform;
        cinemachineVirtualCamera.LookAt = car.transform;        
    }
}
