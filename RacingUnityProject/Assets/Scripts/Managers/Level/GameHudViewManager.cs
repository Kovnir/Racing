using DefaultNamespace;
using JetBrains.Annotations;
using Kovnir.FastTweener;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class GameHudViewManager : MonoBehaviour
{
    [InjectOptional] private LoaderViewManager loaderViewManager;

    [Inject] private DiContainer container;

    private CarController car;
    [Inject] private SignalBus bus;
    
    [SerializeField]
    private TextMeshProUGUI speedText;

    [SerializeField]
    private TextMeshProUGUI loseCheckpointText;
    [SerializeField]
    private TextMeshProUGUI maxSpeedText;
    [SerializeField]
    private float maxArrowAngle;
    [SerializeField]
    private float minArrowAngle;
    [SerializeField]
    private GameObject arrow;


    private FastTween loseCheckpointTween;
    
    private void Awake()
    {
        bus.Subscribe<OnLoseCheckpointSignal>(() =>
        {
            loseCheckpointTween.Kill();
            loseCheckpointText.gameObject.SetActive(true);
            loseCheckpointText.alpha = 1;
            loseCheckpointTween = FastTweener.Schedule(2, () =>
            {
                loseCheckpointTween = FastTweener.Float(1, 0, 2, f => { loseCheckpointText.alpha = f; },
                    () => { loseCheckpointText.gameObject.SetActive(false); });
            });
        });
    }

    private void Start()
    {
        car = container.Resolve<CarController>();
        //do it here because GameHudViewManager can be created before LevelManager, which bind car;
    }

    [UsedImplicitly]
    public void OnBackButtonClick()
    {
        if (loaderViewManager != null)
        {
            loaderViewManager.LoadMainMenu();
        }
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
