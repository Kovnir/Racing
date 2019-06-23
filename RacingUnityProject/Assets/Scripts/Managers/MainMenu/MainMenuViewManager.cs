using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class MainMenuViewManager : MonoBehaviour
{
    [Inject] private LoaderViewManager loaderViewManager;
    [Inject] private PlayerProfileManager playerProfileManager;

    [SerializeField] private LevelButtonView level1Button;
    [SerializeField] private LevelButtonView level2Button;
    [SerializeField] private LevelButtonView level3Button;

    private void Awake()
    {
        var progress = playerProfileManager.GetProgress();
        SetupButton(level1Button, progress.levels, 0);
        SetupButton(level2Button, progress.levels, 1);
        SetupButton(level3Button, progress.levels, 2);
    }

    private void SetupButton(LevelButtonView button, List<PlayerProfile.PlayerProgress.LevelProgress> progressLevels, int i)
    {
        bool isAvailable = i < (progressLevels.Count+1);
        int startCount = 0;
        if (i < progressLevels.Count)
        {
            startCount = progressLevels[i].StarsCount;
        }
        button.Init(isAvailable, startCount);
    }


    [UsedImplicitly]
    public void OnLevelSelected(int levelNum)
    {
        loaderViewManager.LoadLevel(levelNum);
    }
}
