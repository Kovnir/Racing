using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Resources/Settings", menuName = "Game Settings", order = 51)]
public class SettingsInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
//        Container.Bind<Settings>().FromResource("GameSettings");
        Container.Bind<GameSettings>().FromInstance(gameSettings);
    }

    public GameSettings gameSettings;
}
