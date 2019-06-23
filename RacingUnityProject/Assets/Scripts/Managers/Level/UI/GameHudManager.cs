using System;
using DefaultNamespace;
using JetBrains.Annotations;
using Kovnir.FastTweener;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

public class GameHudManager : MonoBehaviour
{
    [InjectOptional] private LoaderViewManager loaderViewManager;

    [Inject] private DiContainer container;

    private CarController car;
    [Inject] private SignalBus bus;
    [Inject] private LevelSettings levelSettings;
    
    [SerializeField]
    private TextMeshProUGUI speedText;

    private float time;

    private bool calculateTime = false;
//    [SerializeField]
//    private TextMeshProUGUI GoldTimeText;
    [SerializeField]
    private TextMeshProUGUI timeText;
    
    [SerializeField]
    private TextMeshProUGUI failedText;
    [SerializeField]
    private TextMeshProUGUI winText;

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
        bus.Subscribe<OnLoseCheckpointSignal>(OnLoseCheckpoint);
        bus.Subscribe<OnRaceStartSignal>(() =>
        {
            nextStarTime = levelSettings.OneStarTime;
            calculateTime = true;
            timeText.gameObject.SetActive(true);
        });
        bus.Subscribe<OnLevelFailedSignal>(signal =>
        {
            calculateTime = false;
            failedText.text = "LEVEL FAILED!\n";
            switch (signal.Reason)
            {
                case OnLevelFailedSignal.FailReason.TimeIsUp:
                    failedText.text += "TIME IS UP\n";
                    break;
                case OnLevelFailedSignal.FailReason.CarCrushed:
                    failedText.text += "CAR WAS CRUSHED\n";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            failedText.text += "(PRESS ESC)";
            failedText.gameObject.SetActive(true);
        });
        bus.Subscribe<OnLevelFinishedSignal>(() =>
        {
            calculateTime = false;
            failedText.text = "LEVEL FINISHED!\n";
            failedText.text += "(PRESS ESC)";
            failedText.gameObject.SetActive(true);
        });
    }


    private void OnLoseCheckpoint()
    {
        loseCheckpointTween.Kill();
        loseCheckpointText.gameObject.SetActive(true);
        loseCheckpointText.alpha = 1;
        loseCheckpointTween = FastTweener.Schedule(2, () =>
        {
            loseCheckpointTween = FastTweener.Float(1, 0, 2, f => { loseCheckpointText.alpha = f; },
                () => { loseCheckpointText.gameObject.SetActive(false); });
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

    private int currentStar = 0;
    private float nextStarTime = 0;
    
    private void Update()
    {
        UpdateSpeed();
        if (calculateTime)
        {
            time += Time.deltaTime;
            timeText.text = time.ToString("0.00");
            
            if (time > nextStarTime)
            {
                bus.Fire<OnStarFailedSignal>();
                currentStar++;
                switch (currentStar)
                {
                    case 1:
                        nextStarTime = levelSettings.TwoStarsTime;
                        break;
                    case 2:
                        nextStarTime = levelSettings.ThreeStarsTime;
                        break;
                    case 3:
                        bus.Fire(new OnLevelFailedSignal(OnLevelFailedSignal.FailReason.TimeIsUp));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void UpdateSpeed()
    {
        var speed = car.GetSpeed();
        speedText.text = speed.CurrentSpeed.ToString("0.00"); //todo optimize
        maxSpeedText.text = speed.MaxSpeed.ToString("0"); //todo optimize
        float percent = speed.CurrentSpeed / speed.MaxSpeed;


        var realValue = Mathf.Lerp(minArrowAngle, maxArrowAngle, percent);

        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, realValue));
    }
}
