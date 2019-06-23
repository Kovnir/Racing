namespace Signals
{
    public class OnCheckpointAchievedSignal
    {
        public readonly CheckPoint CheckPoint;

        public OnCheckpointAchievedSignal(CheckPoint checkPoint)
        {
            CheckPoint = checkPoint;
        }
    }
}