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
//        car = GameObject.FindObjectOfType<CarController>();
        signalBus.Subscribe<SomeSignal>(()=>{Debug.LogError("ONONON!!!");});
//        container.BindSignal<SomeSignal>().ToMethod(() => { });
//        container.BindSignal<SomeSignal>().ToMethod<GameHudViewManager>((manager, signal) => { });
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
        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 180,
            Mathf.Lerp(minArrowAngle, maxArrowAngle, percent)));
    }
}

public class SomeSignal
{
    
}
