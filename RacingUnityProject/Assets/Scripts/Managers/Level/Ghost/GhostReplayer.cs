using System.Collections;
using System.Collections.Generic;
using Managers.Level.Ghost;
using UnityEngine;
using Zenject;

public class GhostReplayer : MonoBehaviour
{
    [Inject] private LevelSettings levelSettings;
    [Inject] private PlayerProfileManager playrPlayerProfileManager;
    [InjectOptional(Id = "levelNum")] private int levelNum;

    [SerializeField] private Transform ghostPrefab;
    private Transform ghost;

    private List<GhostData> records;
    
    void Start()
    {
        var progress = playrPlayerProfileManager.GetProgress();
        if (progress.levels.Count <= levelNum)
        {
            return;
        }
        records = playrPlayerProfileManager.GetProgress().levels[levelNum].Ghost;
        if (records != null && records.Count > 0)
        {
            play = true;
            time = Time.time;
            ghost = Instantiate(ghostPrefab);
        }

        UpdatePosition();
    }
    private float time;
    private int index = 0;

    private bool play;
    // Update is called once per frame
    void Update()
    {
        if (!play)
        {
            return;
        }
        if (Time.time - time > 0.5f)
        {
            UpdatePosition();
            time -= Time.time;
        }
    }

    private void UpdatePosition()
    {
        ghost.transform.position = records[index].Position;
        ghost.transform.rotation = Quaternion.Euler(records[index].Rotation);
        index++;
        if (records.Count == index)
        {
            play = false;
        }
    }
}
