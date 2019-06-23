using System;
using System.Collections.Generic;
using Cinemachine;
using Managers.Camera;
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

    private List<Transform> targets = new List<Transform>();
    private Camera camera;

    private CameraMode cameraMode;

    private void Awake()
    {
        //setup postprocessing
        postProcessingBehaviour = Camera.main.GetComponent<PostProcessingBehaviour>();
        UpdatePostProcessing();
        bus.Subscribe<OnPostProcessingSettingsChangedSignal>(UpdatePostProcessing);
        camera = Camera.main;
        UpdateCameraMode();
        bus.Subscribe<OnCameraModeChangedSignal>(UpdateCameraMode);
    }

    private void UpdateCameraMode()
    {
        cameraMode = playerProfileManager.GetCameraModeState();
        cinemachineVirtualCamera.gameObject.SetActive(cameraMode == CameraMode.Cinemachine);
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
        cinemachineVirtualCamera.m_LookAt = car.transform; //depends on type

    }

    [SerializeField] private Vector3 cameraOffsetPosition = new Vector3(0, 5, -20);
    [SerializeField] private float moveInterpolateFactor = 5;

    private float cameraAdditionalOffset;

    public float cameraAdditionalOffsetPerSecond;

    //how high camera should fly depending on length to car
    public float xCoef;

    private void LateUpdate()
    {
        switch (cameraMode)
        {
            case CameraMode.Cinemachine:
                return;
            case CameraMode.Simple:
                ProcessSimpleCamera(cameraOffsetPosition);
                break;
            case CameraMode.Fallow:
                ProcessFallowCamera();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessSimpleCamera(Vector3 cameraLocalPostition)
    {
        var cameraPos = car.transform.TransformPoint(cameraLocalPostition);
        camera.transform.position =
            Vector3.Slerp(camera.transform.position, cameraPos, moveInterpolateFactor * Time.deltaTime);

        camera.transform.LookAt(car.transform);
    }

    private void ProcessFallowCamera()
    {
        //if all object are out of screen - zoom out
        //if all object are in screen, and no objects in 20% border - zoom in
        //else - do nothing
        CalculateZoom();

        Vector3 localCameraPos =
            cameraOffsetPosition - new Vector3(0, xCoef * cameraAdditionalOffset, cameraAdditionalOffset);
        ProcessSimpleCamera(localCameraPos);
    }

    private void CalculateZoom()
    {
        int inCenterCount = 0;
        int outOfScreenCount = 0;
        int neutralCount = 0;
        foreach (var target in targets)
        {
            Vector3 screenPoint = camera.WorldToViewportPoint(target.position);
            bool onScreen = screenPoint.z > 0f && screenPoint.x > 0.2f && screenPoint.x < 0.8f &&
                            screenPoint.y > 0.2f &&
                            screenPoint.y < 0.8f;
            if (onScreen)
            {
                inCenterCount++;
                continue;
            }

            bool outOfScreen = !(screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 &&
                                 screenPoint.y < 1);
            if (outOfScreen)
            {
                outOfScreenCount++;
                break;
            }

            neutralCount++;
        }

        if (outOfScreenCount > 0)
        {
            cameraAdditionalOffset += cameraAdditionalOffsetPerSecond * Time.deltaTime;
        }
        else
        {
            if (neutralCount == 0 && inCenterCount > 0)
            {
                if (cameraAdditionalOffset > 0)
                {
                    cameraAdditionalOffset -= cameraAdditionalOffsetPerSecond * Time.deltaTime;
                }

                if (cameraAdditionalOffset < 0)
                {
                    cameraAdditionalOffset = 0;
                }
            }
        }
    }

    public void AddTarget(Transform transform)
    {
        targets.Add(transform);
    }

    public void RemoveTarget(Transform transform)
    {
        targets.Remove(transform);
    }
}