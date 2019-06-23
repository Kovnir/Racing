using System;
using System.Collections;
using System.Collections.Generic;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

public class StartsUI : MonoBehaviour
{
    [Header("Titles")]
    [SerializeField] private TextMeshProUGUI stars1Title;
    [SerializeField] private TextMeshProUGUI stars2Title;
    [SerializeField] private TextMeshProUGUI stars3Title;
    
    [Header("Values")]
    [SerializeField] private TextMeshProUGUI stars1;
    [SerializeField] private TextMeshProUGUI stars2;
    [SerializeField] private TextMeshProUGUI stars3;

    [Inject] private LevelSettings settings;
    [Inject] private SignalBus bus;

    [SerializeField] private Color failedColor;
    [SerializeField] private Color nextColor;
    
    private int failed;
    private void Awake()
    {
        failed = 0;
        if (settings != null)
        {
            stars1.text = settings.OneStarTime.ToString("0.00");
            stars2.text = settings.TwoStarsTime.ToString("0.00");
            stars3.text = settings.ThreeStarsTime.ToString("0.00");
            stars1.color = nextColor;
            stars1Title.color = nextColor;
        }
        bus.Subscribe<OnStarFailedSignal>(() =>
        {
            switch (failed)
            {
                case 0:
                    stars1.color = failedColor;
                    stars1Title.color = failedColor;
                    stars2.color = nextColor;
                    stars2Title.color = nextColor;
                    break;
                case 1:
                    stars2.color = failedColor;
                    stars2Title.color = failedColor;
                    stars3.color = nextColor;
                    stars3Title.color = nextColor;
                    break;
                case 2:
                    stars3.color = failedColor;
                    stars3Title.color = failedColor;
                    break;
                default:
                    throw new ArgumentException();
            }

            failed++;
        });
    }
}
