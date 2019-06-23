using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnPoint : MonoBehaviour
{
    [Inject] private LevelManager levelManager;

    void Awake()
    {
        levelManager.RegisterSpawnPoint(this);
        gameObject.SetActive(false);
    }
}
