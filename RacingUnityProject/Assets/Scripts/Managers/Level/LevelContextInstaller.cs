using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelContextInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LevelManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CameraManager>().FromComponentInHierarchy().AsSingle();
    }
}
