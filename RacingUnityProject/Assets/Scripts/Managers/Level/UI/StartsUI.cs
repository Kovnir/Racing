using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class StartsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stars1;
    [SerializeField] private TextMeshProUGUI stars2;
    [SerializeField] private TextMeshProUGUI stars3;

    [InjectOptional] private LevelSettings settings;

    private void Awake()
    {
        if (settings != null)
        {
            stars1.text = settings.OneStarTime.ToString("0.00");
            stars2.text = settings.TwoStarsTime.ToString("0.00");
            stars3.text = settings.ThreeStarsTime.ToString("0.00");
        }
    }
}
