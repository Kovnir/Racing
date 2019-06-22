using DefaultNamespace;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class GameHudViewManager : MonoBehaviour
{
    [InjectOptional] private LoaderViewManager loaderViewManager;

    [Inject] private SignalBus signalBus;

    [Inject] private DiContainer container;

    [Inject] private CarController car;
    
    [SerializeField]
    private TextMeshProUGUI speedText;
    [SerializeField]
    private TextMeshProUGUI maxSpeedText;
    [SerializeField]
    private float maxArrowAngle;
    [SerializeField]
    private float minArrowAngle;
    [SerializeField]
    private GameObject arrow;

    
    private void Awake()
    {
//        signalBus.Subscribe<SomeSignal>(()=>{Debug.LogError("ONONON!!!");});
    }
    
    [UsedImplicitly]
    public void OnBackButtonClick()
    {
        if (loaderViewManager != null)
        {
            loaderViewManager.LoadMainMenu();
        }
        
        signalBus.Fire<SomeSignal>();
    }

    private void Update()
    {
        var speed = car.GetSpeed();
        speedText.text = speed.CurrentSpeed.ToString("0.00");//todo optimize
        maxSpeedText.text = speed.MaxSpeed.ToString("0"); //todo optimize
        float percent = speed.CurrentSpeed / speed.MaxSpeed;
        
        
        var realValue = Mathf.Lerp(minArrowAngle, maxArrowAngle, percent);
        
        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 180,realValue));
    }
}

public class SomeSignal
{
    
}
