namespace PomodoroTimer.Utilities
{
    public class CountDownTimerData
    {
        public DateTime StartTime { get; set; }
        public DateTime PauseTime { get; set; }
        public DateTime EndTime { get; set; }
        public CountDownTimerState TimerState { get; set; }
    }

    public enum CountDownTimerState
    {
        Running,
        Paused,
        Stopped
    }
}
