namespace Signals
{
    public class OnLevelFailedSignal
    {

        public enum FailReason
        {
            TimeIsUp,
            CarCrushed,
        }

        public readonly FailReason Reason;
        
        public OnLevelFailedSignal(FailReason reason)
        {
            Reason = reason;
        }
    }
}