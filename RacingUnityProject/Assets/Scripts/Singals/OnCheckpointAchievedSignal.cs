using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCheckpointAchievedSignal
{
    public readonly CheckPoint CheckPoint;
    public OnCheckpointAchievedSignal(CheckPoint checkPoint)
    {
        CheckPoint = checkPoint;
    }
}
